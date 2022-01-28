using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsCashCollection
    {
        public string CRVNumber { get; set; }
        public int CustomerID { get; set; }
        public DateTime CollectionDate { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public int UserID { get; set; }

        public clsCashCollection()
        {
            CRVNumber = "";
            CustomerID = 0;
            CollectionDate = DateTime.Now;
            Amount = 0;
            Remark = "";
            UserID = 0;
        }
    }
}
