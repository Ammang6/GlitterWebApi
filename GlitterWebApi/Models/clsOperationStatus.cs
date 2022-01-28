using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsOperationStatus
    {
        public string OperationNumber { get; set; }
        public DateTime EventDate { get; set; }
        public string OperationStatus { get; set; }
        public string Remark { get; set; }
        public int RowIndex { get; set; }
        public int UserID { get; set; }

        public clsOperationStatus()
        {
            OperationNumber = "";
            EventDate = DateTime.Now;
            OperationStatus = "";
            Remark = "";
            RowIndex = 0;
            UserID = 0;
        }
    }
}
