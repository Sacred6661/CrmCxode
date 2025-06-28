using CrmCxode.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Services
{
    public interface ICrm
    {
        public Task<List<CrmTicket>> GetTicketsAsync();
    }
}
