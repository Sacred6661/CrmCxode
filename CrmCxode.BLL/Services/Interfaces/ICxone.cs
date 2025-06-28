using CrmCxode.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Services
{
    public interface ICxone
    {
        public Task SendTicketAsync(CxoneTicket ticket);
    }
}
