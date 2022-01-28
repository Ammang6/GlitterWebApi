using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsPort
    {
        public int PortID { get; set; }
        public string PortName { get; set; }
        public string Country { get; set; }
        public int UserID { get; set; }

        public clsPort()
        {
            PortID = 0;
            PortName = "";
            Country = "";
            UserID = 0;
        }
    }
}
