using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsExpenseList
    {
        public int ExpenseID { get; set; }
        public string ExpenseDescription { get; set; }
        public int UserID { get; set; }

        public clsExpenseList()
        {
            ExpenseID = 0;
            ExpenseDescription = "";
            UserID = 0;
        }
    }
}
