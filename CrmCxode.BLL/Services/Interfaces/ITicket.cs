using CrmCxode.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Services
{
    public interface ITicket
    {
        public Task<List<CrmTicket>> GetCrmTicketsAsync();
        public Task SendTicketsAsync(List<CrmTicket> tickets);
        public Task SendTicketsParallelAsync(List<CrmTicket> tickets);
    }
}
