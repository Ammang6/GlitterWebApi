using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsExpense
    {
        public int AutoID { get; set; }
        public int ExpenseID { get; set; }
        public string OperationNumber { get; set; }
        public int ServiceProviderID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public int UserID { get; set; }

        public clsExpense()
        {
            AutoID = 0;
            ExpenseID = 0;
            OperationNumber = "";
            ServiceProviderID = 0;
            ExpenseDate = DateTime.Now;
            Amount = 0;
            Remark = "";
            UserID = 0;
        }
    }
}
