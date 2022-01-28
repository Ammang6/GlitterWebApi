using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlitterWebApi.Models
{
    public class clsOperation
    {
        public string OperationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public int CustomerID { get; set; }
        public int CommodityID { get; set; }
        public int LoadPort { get; set; }
        public int DischargePort { get; set; }
        public string OperationType { get; set; }
        public string PermitNumber { get; set; }
        public string DeclaratonNumber { get; set; }
        public string TypeOfDeclaration { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string OrderType { get; set; }
        public string ShippingInstructionNo { get; set; }
        public string Remark { get; set; }
        public int UserID { get; set; }

        public clsOperation()
        {
            OperationNumber = "";
            StartDate = DateTime.Now;
            CustomerID = 0;
            CommodityID = 0;
            LoadPort = 0;
            DischargePort = 0;
            OperationType = "";
            PermitNumber = "";
            DeclaratonNumber = "";
            TypeOfDeclaration = "";
            CustomerReferenceNumber = "";
            OrderType = "";
            ShippingInstructionNo = "";
            Remark = "";
            UserID = 0;
        }
    }
}
