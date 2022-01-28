using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsCashPayment
    {
        public string PaymentVoucherNo { get; set; }
        public int ServiceProviderID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public int UserID { get; set; }

        public clsCashPayment()
        {
            PaymentVoucherNo = "";
            ServiceProviderID = 0;
            PaymentDate = DateTime.Now;
            Amount = 0;
            Remark = "";
            UserID = 0;
        }
    }
}
