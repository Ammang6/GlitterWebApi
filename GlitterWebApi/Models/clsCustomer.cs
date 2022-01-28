using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsCustomer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public string ContactPersons { get; set; }
        public string Address { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string VATRegNo { get; set; }
        public string TINNo { get; set; }
        public int UserID { get; set; }

        public clsCustomer()
        {
            CustomerID = 0;
            CustomerName = "";
            CustomerType = "";
            ContactPersons = "";
            Address = "";
            Telephone1 = "";
            Telephone2 = "";
            Mobile = "";
            Email = "";
            VATRegNo = "";
            TINNo = "";
            UserID = 0;
        }
    }
}
