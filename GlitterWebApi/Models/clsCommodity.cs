using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsCommodity
    {
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public string Category { get; set; }
        public string HSCode { get; set; }
        public int UserID { get; set; }

        public clsCommodity()
        {
            CommodityID = 0;
            CommodityName = "";
            Category = "";
            HSCode = "";
            UserID = 0;
        }
    }
}
