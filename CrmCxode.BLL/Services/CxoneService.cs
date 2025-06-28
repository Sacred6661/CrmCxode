using CrmCxode.BLL.Models;
using CrmCxode.Contracts;
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

        public CxoneService(HttpClient httpClient, MainSettings settings)
        {
            _httpClient = httpClient;
            _setting = settings;
        }

        public async Task SendTicketAsync(CxoneTicket ticket)
        {
            var json = JsonSerializer.Serialize(ticket);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_setting.CxoneApi, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
