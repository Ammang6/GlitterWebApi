using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GlitterWebApi.Models;

namespace GlitterWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        private CustomerController myCustomer;
        private CommodityController myCommodity;
        private PortController myPort;

        public OperationController(IConfiguration configuration)
        {
            _configuration = configuration;

            myValidator = new Validator(configuration);
            myCustomer = new CustomerController(configuration);
            myCommodity = new CommodityController(configuration);
            myPort = new PortController(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT tblOperation.OperationNumber, tblOperation.StartDate, tblOperation.CustomerID, " +
                            "tblCustomer.CustomerName, tblOperation.CommodityID, tblCommodity.CommodityName, " +
                            "tblOperation.LoadPort, tblLoadPort.PortName AS LoadPortName, tblOperation.DischargePort, " +
                            "tblDischargePort.PortName AS DischargePortName, tblOperation.OperationType, " +
                            "tblOperation.PermitNumber, tblOperation.DeclaratonNumber, tblOperation.TypeOfDeclaration, " +
                            "tblOperation.CustomerReferenceNumber, tblOperation.OrderType, tblOperation.ShippingInstructionNo, " +
                            "tblOperation.Remark FROM tblOperation INNER JOIN tblCustomer ON " +
                            "tblOperation.CustomerID = tblCustomer.CustomerID INNER JOIN tblCommodity ON " +
                            "tblOperation.CommodityID = tblCommodity.CommodityID INNER JOIN tblPort AS tblLoadPort ON " +
                            "tblOperation.LoadPort = tblLoadPort.PortID INNER JOIN tblPort AS tblDischargePort ON " +
                            "tblOperation.DischargePort = tblDischargePort.PortID";
            DataTable tblOperation = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readOperation;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readOperation = myCommand.ExecuteReader();
                    tblOperation.Load(readOperation);
                    readOperation.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblOperation);
            

        }

        [HttpPost]
        public JsonResult Post(clsOperation objOperation)
        {
            if (myCustomer.CustomerIDFound(objOperation.CustomerID) == false)
            {
                return new JsonResult("No Record Added, Customer ID Not Found!!");
            }
            else if (myCommodity.CommodityIDFound(objOperation.CommodityID) == false)
            {
                return new JsonResult("No Record Added, Commodity ID Not Found!!");
            }
            else if (myPort.PortIDFound(objOperation.LoadPort) == false)
            {
                return new JsonResult("No Record Added, Load Port ID Not Found!!");
            }
            else if (myPort.PortIDFound(objOperation.DischargePort) == false)
            {
                return new JsonResult("No Record Added, Discharge Port ID Not Found!!");
            }
            else
            {
                string strSQL = "INSERT INTO tblOperation " +
                                "(OperationNumber, StartDate, CustomerID, CommodityID, LoadPort, " +
                                 "DischargePort, OperationType, PermitNumber, DeclaratonNumber, " +
                                 "TypeOfDeclaration, CustomerReferenceNumber, OrderType, " +
                                 "ShippingInstructionNo, Remark, UserID) " +
                            "VALUES " +
                                "(@OperationNumber, @StartDate, @CustomerID, @CommodityID, @LoadPort, " +
                                 "@DischargePort, @OperationType, @PermitNumber, @DeclaratonNumber, " +
                                 "@TypeOfDeclaration, @CustomerReferenceNumber, @OrderType, " +
                                 "@ShippingInstructionNo, @Remark, @UserID)";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@OperationNumber", objOperation.OperationNumber);
                        myCommand.Parameters.AddWithValue("@StartDate", objOperation.StartDate);
                        myCommand.Parameters.AddWithValue("@CustomerID", objOperation.CustomerID);
                        myCommand.Parameters.AddWithValue("@CommodityID", objOperation.CommodityID);
                        myCommand.Parameters.AddWithValue("@LoadPort", objOperation.LoadPort);
                        myCommand.Parameters.AddWithValue("@DischargePort", objOperation.DischargePort);
                        myCommand.Parameters.AddWithValue("@OperationType", objOperation.OperationType);
                        myCommand.Parameters.AddWithValue("@PermitNumber", objOperation.PermitNumber);
                        myCommand.Parameters.AddWithValue("@DeclaratonNumber", objOperation.DeclaratonNumber);
                        myCommand.Parameters.AddWithValue("@TypeOfDeclaration", objOperation.TypeOfDeclaration);
                        myCommand.Parameters.AddWithValue("@CustomerReferenceNumber", objOperation.CustomerReferenceNumber);
                        myCommand.Parameters.AddWithValue("@OrderType", objOperation.OrderType);
                        myCommand.Parameters.AddWithValue("@ShippingInstructionNo", objOperation.ShippingInstructionNo);
                        myCommand.Parameters.AddWithValue("@Remark", objOperation.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objOperation.UserID);

                        AffectedRecords = myCommand.ExecuteNonQuery();
                        myConn.Close();
                    }
                }
                if (AffectedRecords > 0)
                    return new JsonResult(AffectedRecords + " Record Added Successfully");
                else
                    return new JsonResult("No Record Added!!");
            }
        }

        [HttpPut]
        public JsonResult Put(clsOperation objOperation)
        {
            if (myCustomer.CustomerIDFound(objOperation.CustomerID) == false)
            {
                return new JsonResult("No Record Updated, Customer ID Not Found!!");
            }
            else if (myCommodity.CommodityIDFound(objOperation.CommodityID) == false)
            {
                return new JsonResult("No Record Updated, Commodity ID Not Found!!");
            }
            else if (myPort.PortIDFound(objOperation.LoadPort) == false)
            {
                return new JsonResult("No Record Updated, Load Port ID Not Found!!");
            }
            else if (myPort.PortIDFound(objOperation.DischargePort) == false)
            {
                return new JsonResult("No Record Updated, Discharge Port ID Not Found!!");
            }
            else
            {
                string strSQL = "UPDATE tblOperation SET " +
                                "StartDate = @StartDate, " +
                                "CustomerID = @CustomerID, " +
                                "CommodityID = @CommodityID, " +
                                "LoadPort = @LoadPort, " +
                                "DischargePort = @DischargePort, " +
                                "OperationType = @OperationType, " +
                                "PermitNumber = @PermitNumber, " +
                                "DeclaratonNumber = @DeclaratonNumber, " +
                                "TypeOfDeclaration = @TypeOfDeclaration, " +
                                "CustomerReferenceNumber = @CustomerReferenceNumber, " +
                                "OrderType = @OrderType, " +
                                "ShippingInstructionNo = @ShippingInstructionNo, " +
                                "Remark = @Remark, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE OperationNumber = @OperationNumber";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@OperationNumber", objOperation.OperationNumber);
                        myCommand.Parameters.AddWithValue("@StartDate", objOperation.StartDate);
                        myCommand.Parameters.AddWithValue("@CustomerID", objOperation.CustomerID);
                        myCommand.Parameters.AddWithValue("@CommodityID", objOperation.CommodityID);
                        myCommand.Parameters.AddWithValue("@LoadPort", objOperation.LoadPort);
                        myCommand.Parameters.AddWithValue("@DischargePort", objOperation.DischargePort);
                        myCommand.Parameters.AddWithValue("@OperationType", objOperation.OperationType);
                        myCommand.Parameters.AddWithValue("@PermitNumber", objOperation.PermitNumber);
                        myCommand.Parameters.AddWithValue("@DeclaratonNumber", objOperation.DeclaratonNumber);
                        myCommand.Parameters.AddWithValue("@TypeOfDeclaration", objOperation.TypeOfDeclaration);
                        myCommand.Parameters.AddWithValue("@CustomerReferenceNumber", objOperation.CustomerReferenceNumber);
                        myCommand.Parameters.AddWithValue("@OrderType", objOperation.OrderType);
                        myCommand.Parameters.AddWithValue("@ShippingInstructionNo", objOperation.ShippingInstructionNo);
                        myCommand.Parameters.AddWithValue("@Remark", objOperation.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objOperation.UserID);

                        AffectedRecords = myCommand.ExecuteNonQuery();
                        myConn.Close();
                    }
                }
                if (AffectedRecords > 0)
                    return new JsonResult(AffectedRecords + " Record Updated Successfully");
                else
                    return new JsonResult("No Record Updated!!");
            }
        }


        [HttpDelete("{_OperationNumber}")]
        public JsonResult Delete(string _OperationNumber)
        {
            string strSQL = "DELETE FROM tblOperation WHERE OperationNumber = @OperationNumber";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@OperationNumber", _OperationNumber);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }

        public Boolean OperationNumberFound(string _OperationNumber)
        {
            return myValidator.FindInTable("tblOperation", "OperationNumber", _OperationNumber);
        }

        public int FindNumberOfOperations()
        {
            return myValidator.FindRecordCount("tblOperation", "OperationNumber");
        }

    }
}
