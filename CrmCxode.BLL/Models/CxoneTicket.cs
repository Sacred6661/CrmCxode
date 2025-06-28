using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Models
{
    public class CxoneTicket
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime Created { get; set; }
        public List<string> Tags { get; set; }
    }
}
