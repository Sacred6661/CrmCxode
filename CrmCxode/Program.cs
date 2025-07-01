// See https://aka.ms/new-console-template for more information


using CrmCxode.BLL.Services;
using CrmCxode.Contracts;
using CrmCxode.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

//// Register services
//var serviceProvider = new ServiceCollection()
//    //Adding services here
//    .AddHttpClient()
//    .AddTransient<ICrm, CrmService>()
//    .BuildServiceProvider();

//var crmService = serviceProvider.GetRequiredService<ICrm>();


var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// connection to NLog via appsettings
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Logging.AddNLog(builder.Configuration);

var settings = builder.Configuration.GetSection("MainSettings").Get<MainSettings>();
builder.Services.AddSingleton(settings);

builder.Services.AddTransient<ICrm, CrmService>();
builder.Services.AddTransient<ICxone, CxoneService>();
builder.Services.AddTransient<ITicket, TicketService>();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Note that MaxConcurrentRequests should be changed base on max connection in one time
int maxConcurrent = builder.Configuration.GetValue<int>("MainSettings:MaxConcurrentRequests");
builder.Services.AddSingleton(new SemaphoreSlim(maxConcurrent));

var host = builder.Build();

using var scope = host.Services.CreateScope();

var logger = host.Services.GetRequiredService<ILogger<Program>>();


// catching global unhandled exceptions
AppDomain.CurrentDomain.UnhandledException += (s, e) =>
{
    logger.LogError(e.ExceptionObject as Exception, "Unhandled exception");
    LogManager.Shutdown();
};

// Catches asynchronous exceptions that were not awaited
TaskScheduler.UnobservedTaskException += (s, e) =>
{
    logger.LogError(e.Exception, "Unobserved task exception");
    e.SetObserved();
};

var ticketService = scope.ServiceProvider.GetRequiredService<ITicket>();

var tickets = await ticketService.GetCrmTicketsAsync();

// This allows to use code without parallels connection
//await ticketService.SendTicketsAsync(tickets);


// parallel using.
await ticketService.SendTicketsParallelAsync(tickets);