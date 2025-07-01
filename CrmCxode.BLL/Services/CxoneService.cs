using CrmCxode.BLL.Models;
using CrmCxode.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Services
{
    public class CxoneService : ICxone
    {
        private readonly HttpClient _httpClient;
        private readonly MainSettings _setting;
        private readonly ILogger<CxoneService> _logger;


        public CxoneService(HttpClient httpClient, MainSettings settings, ILogger<CxoneService> logger)
        {
            _httpClient = httpClient;
            _setting = settings;
            _logger = logger;
        }

        public async Task SendTicketAsync(CxoneTicket ticket)
        {
            try
            {
                _logger.LogInformation($"Send Ticket: {ticket.Subject} ticketID: {ticket.Id}");

                var json = JsonSerializer.Serialize(ticket);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_setting.CxoneApi, content);
                response.EnsureSuccessStatusCode();

                _logger.LogInformation($"Ticket Subject {ticket.Subject} is successfully sent!");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while sending ticketID: {ticket.Id} , Subject:  { ticket.Subject}" );
            }

        }
    }
}
