using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsServiceProvider
    {
        public int ServiceProviderID { get; set; }
        public string ServiceProviderName { get; set; }
        public string ServiceType { get; set; }
        public string ContactPersons { get; set; }
        public string Address { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string VATRegNo { get; set; }
        public string TINNo { get; set; }
        public int UserID { get; set; }

        public clsServiceProvider()
        {
            ServiceProviderID = 0;
            ServiceProviderName = "";
            ServiceType = "";
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
