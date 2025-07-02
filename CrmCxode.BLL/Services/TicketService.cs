using AutoMapper;
using CrmCxode.BLL.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
        private readonly SemaphoreSlim _semaphore;

        private int _activeCount = 0;
        private int _maxActiveCount = 0;


        public TicketService(ICrm crm, ICxone cxone, IMapper mapper, ILogger<TicketService> logger, SemaphoreSlim semaphore)
        {
            _crmService = crm;
            _cxoneService = cxone;
            _mapper = mapper;
            _logger = logger;
            _semaphore = semaphore;
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


        public async Task SendTicketsAsync(List<CrmTicket> tickets)
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


        // This is used for parallels ticket sending
        public async Task SendTicketsParallelAsync(List<CrmTicket> tickets)
        {

            var tasks = new List<Task>();

            var cxoneTickets = _mapper.Map<List<CxoneTicket>>(tickets);

            foreach (var cxoneTicket in cxoneTickets)
            {
                tasks.Add(ProcessTicketParallelAsync(cxoneTicket));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation($"Max one time active count {_maxActiveCount}");
        }

        // This fucntion used to send ticket and make some check with semaphores
        private async Task ProcessTicketParallelAsync(CxoneTicket ticket)
        {
            _logger.LogInformation("Task {Id} waiting for the semaphore...", ticket.Id);

            await _semaphore.WaitAsync();

            try
            {
                int current = Interlocked.Increment(ref _activeCount);
                UpdateMaxActive(current);

                _logger.LogInformation($"Task {ticket.Id}, Subject: {ticket.Subject} running");
                await _cxoneService.SendTicketAsync(ticket);
                // this is added just to check that semaphores is working. pause during execution
                //await Task.Delay(1500);

                _logger.LogInformation($"Ticket id {ticket.Id} is sent.");
            }
            finally
            {
                Interlocked.Decrement(ref _activeCount);
                _semaphore.Release();
            }

        }

        private void UpdateMaxActive(int current)
        {
            int initial, computed;
            do
            {
                initial = _maxActiveCount;
                computed = Math.Max(initial, current);
            }
            while (initial != Interlocked.CompareExchange(ref _maxActiveCount, computed, initial));
        }
    }
}
