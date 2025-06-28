// See https://aka.ms/new-console-template for more information


using CrmCxode.BLL.Services;
using CrmCxode.Contracts;
using CrmCxode.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//// Register services
//var serviceProvider = new ServiceCollection()
//    //Adding services here
//    .AddHttpClient()
//    .AddTransient<ICrm, CrmService>()
//    .BuildServiceProvider();

//var crmService = serviceProvider.GetRequiredService<ICrm>();


var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var settings = builder.Configuration.GetSection("MainSettings").Get<MainSettings>();
builder.Services.AddSingleton(settings);

builder.Services.AddTransient<ICrm, CrmService>();
builder.Services.AddTransient<ICxone, CxoneService>();
builder.Services.AddTransient<ITicket, TicketService>();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var host = builder.Build();

using var scope = host.Services.CreateScope();

Console.WriteLine("Hello, World!");

var ticketService = scope.ServiceProvider.GetRequiredService<ITicket>();

var tickets = await ticketService.GetCrmTicketsAsync();


await ticketService.SendTickets(tickets);

//await crmService.GetTicketsAsync();
