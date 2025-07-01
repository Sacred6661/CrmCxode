using AutoMapper;
using CrmCxode.BLL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Services
{
    public class TicketService : ITicket
    {
        private readonly ICrm _crmService;
        private readonly ICxone _cxoneService;
        private readonly IMapper _mapper;
        private readonly ILogger<TicketService> _logger;


        public TicketService(ICrm crm, ICxone cxone, IMapper mapper, ILogger<TicketService> logger)
        {
            _crmService = crm;
            _cxoneService = cxone;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CrmTicket>> GetCrmTicketsAsync()
        {
            var result = await _crmService.GetTicketsAsync();

            if(result == null)
            {
                _logger.LogInformation("There is no CRM tickets to work with.");
                return null;
            }

            return result;
        }

        public async Task SendTickets(List<CrmTicket> tickets)
        {
            if(tickets == null || tickets.Count == 0)
            {
                _logger.LogInformation("Tickets list is empty");
                return;
            }
            var cxoneTickets = _mapper.Map<List<CxoneTicket>>(tickets);

            _logger.LogInformation("Sart tickets sending.");

            // Send cxode one by one to the server
            foreach (var ticket in cxoneTickets)
            {     
                await _cxoneService.SendTicketAsync(ticket);
            }
        }
    }
}
