using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.BLL.Models
{
    public class CrmTicket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public DateTime CreatedAt { get; set; }
        public List<string> Tags { get; set; }
    }
}
