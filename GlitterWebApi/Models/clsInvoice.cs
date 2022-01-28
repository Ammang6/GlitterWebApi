using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsInvoice
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string OperationNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string Remark { get; set; }
        public int UserID { get; set; }

        public clsInvoice()
        {
            InvoiceNo = "";
            InvoiceDate = DateTime.Now;
            OperationNumber = "";
            InvoiceAmount = 0;
            Remark = "";
            UserID = 0;
        }
    }
}
