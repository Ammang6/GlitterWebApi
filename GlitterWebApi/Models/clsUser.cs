using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid Salt { get; set; }
    }
}
