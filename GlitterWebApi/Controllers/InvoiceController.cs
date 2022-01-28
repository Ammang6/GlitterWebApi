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
    public class InvoiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        private OperationController myOperation;

        public InvoiceController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);

            myOperation = new OperationController(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT tblInvoice.InvoiceNo, tblInvoice.InvoiceDate, tblInvoice.OperationNumber, " +
                            "tblOperation.CustomerID, tblCustomer.CustomerName, tblInvoice.InvoiceAmount, " +
                            "tblInvoice.Remark FROM tblInvoice INNER JOIN tblOperation ON " +
                            "tblInvoice.OperationNumber = tblOperation.OperationNumber INNER JOIN " +
                            "tblCustomer ON tblOperation.CustomerID = tblCustomer.CustomerID ORDER BY InvoiceNo";
            DataTable tblInvoice = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readInvoice;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readInvoice = myCommand.ExecuteReader();
                    tblInvoice.Load(readInvoice);
                    readInvoice.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblInvoice);
        }


        [HttpPost]
        public JsonResult Post(clsInvoice objInvoice)
        {
            if (myOperation.OperationNumberFound(objInvoice.OperationNumber))
            {
                string strSQL = "INSERT INTO tblInvoice " +
                                "(InvoiceNo, InvoiceDate, OperationNumber, InvoiceAmount, " +
                                 "Remark, UserID) " +
                            "VALUES " +
                                "(@InvoiceNo, @InvoiceDate, @OperationNumber, @InvoiceAmount, " +
                                 "@Remark, @UserID)";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@InvoiceNo", objInvoice.InvoiceNo);
                        myCommand.Parameters.AddWithValue("@InvoiceDate", objInvoice.InvoiceDate);
                        myCommand.Parameters.AddWithValue("@OperationNumber", objInvoice.OperationNumber);
                        myCommand.Parameters.AddWithValue("@InvoiceAmount", objInvoice.InvoiceAmount);
                        myCommand.Parameters.AddWithValue("@Remark", objInvoice.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objInvoice.UserID);

                        AffectedRecords = myCommand.ExecuteNonQuery();
                        myConn.Close();
                    }
                }
                if (AffectedRecords > 0)
                    return new JsonResult(AffectedRecords + " Record Added Successfully");
                else
                    return new JsonResult("No Record Added!!");
            }
            else
                return new JsonResult("No Record Added, Operation Number Not Found!!");
        }

        [HttpPut]
        public JsonResult Put(clsInvoice objInvoice)
        {
            if (myOperation.OperationNumberFound(objInvoice.OperationNumber))
            {
                string strSQL = "UPDATE tblInvoice SET " +
                                "InvoiceDate = @InvoiceDate, " +
                                "OperationNumber = @OperationNumber, " +
                                "InvoiceAmount = @InvoiceAmount, " +
                                "Remark = @Remark, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE InvoiceNo = @InvoiceNo";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@InvoiceNo", objInvoice.InvoiceNo);
                        myCommand.Parameters.AddWithValue("@InvoiceDate", objInvoice.InvoiceDate);
                        myCommand.Parameters.AddWithValue("@OperationNumber", objInvoice.OperationNumber);
                        myCommand.Parameters.AddWithValue("@InvoiceAmount", objInvoice.InvoiceAmount);
                        myCommand.Parameters.AddWithValue("@Remark", objInvoice.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objInvoice.UserID);

                        AffectedRecords = myCommand.ExecuteNonQuery();
                        myConn.Close();
                    }
                }
                if (AffectedRecords > 0)
                    return new JsonResult(AffectedRecords + " Record Updated Successfully");
                else
                    return new JsonResult("No Record Updated!!");
            }
            else
                return new JsonResult("No Record Updated, Operation Number Not Found!!");
        }

        [HttpDelete("{_InvoiceNo}")]
        public JsonResult Delete(string _InvoiceNo)
        {
            string strSQL = "DELETE FROM tblInvoice WHERE InvoiceNo = @InvoiceNo";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@InvoiceNo", _InvoiceNo);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }


        public int FindNumberOfInvoice()
        {
            return myValidator.FindRecordCount("tblInvoice", "InvoiceNo");
        }

    }
}
