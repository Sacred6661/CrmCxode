using CrmCxode.BLL.Models;
using CrmCxode.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Services
{
    public class CrmService : ICrm
    {
        private readonly HttpClient _httpClient;
        private readonly MainSettings _settings;
        private readonly ILogger<CrmService> _logger;

        public CrmService(HttpClient httpClient, MainSettings settings, ILogger<CrmService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }


        public async Task<List<CrmTicket>> GetTicketsAsync()
        {
            // options for correct JSON parsing
            var jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            try
            {
                var response = await _httpClient.GetAsync(_settings.CrmApi);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var crmTickes = JsonSerializer.Deserialize<List<CrmTicket>>(json, jsonOption);

                if (crmTickes == null || crmTickes.Count == 0)
                    return null;

                // added custom tags if there is no tags in object
                foreach (var ticket in crmTickes)
                {
                    if (ticket.Tags == null || ticket.Tags.Count == 0)
                        ticket.Tags = ["test", "example", "sample"];
                }

                return crmTickes;
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while retrieving CRM tickets.", ex);
            }
        }
    }
}
