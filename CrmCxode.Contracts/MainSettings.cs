using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.Contracts
{
    // class to use class with appsettings
    public class MainSettings
    {
        public string CrmApi { get; set; }
        public string CxoneApi { get; set; }
        public string LogFilePath { get; set; }
    }
}
