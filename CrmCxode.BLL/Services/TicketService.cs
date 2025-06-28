using AutoMapper;
using CrmCxode.BLL.Models;
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

        public TicketService(ICrm crm, ICxone cxone, IMapper mapper)
        {
            _crmService = crm;
            _cxoneService = cxone;
            _mapper = mapper;
        }

        public async Task<List<CrmTicket>> GetCrmTicketsAsync()
        {
            return await _crmService.GetTicketsAsync();
        }

        public async Task SendTickets(List<CrmTicket> tickets)
        {
            var cxoneTickets = _mapper.Map<List<CxoneTicket>>(tickets);

            // Send cxode one by one to the server
            foreach (var ticket in cxoneTickets)
            {
                Console.WriteLine($"SendTicket: {ticket.Subject}");
                await _cxoneService.SendTicketAsync(ticket);
                Console.WriteLine("Sent!");
            }

            Console.WriteLine("test");
        }
    }
}
