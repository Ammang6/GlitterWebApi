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
    public class CashPaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        private ServiceProviderController myServiceProvider;

        public CashPaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);

            myServiceProvider = new ServiceProviderController(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT tblCashPayment.PaymentVoucherNo, tblCashPayment.ServiceProviderID, " +
                            "tblServiceProvider.ServiceProviderName, tblCashPayment.PaymentDate, " +
                            "tblCashPayment.Amount, tblCashPayment.Remark FROM tblCashPayment " +
                            "INNER JOIN tblServiceProvider ON " +
                            "tblCashPayment.ServiceProviderID = tblServiceProvider.ServiceProviderID ORDER BY PaymentVoucherNo";
            DataTable tblCashPayment = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readCash;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readCash = myCommand.ExecuteReader();
                    tblCashPayment.Load(readCash);
                    readCash.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblCashPayment);
        }

        [HttpPost]
        public JsonResult Post(clsCashPayment objCashPayment)
        {
            if (myServiceProvider.ServiceProviderIDFound(objCashPayment.ServiceProviderID))
            {
                string strSQL = "INSERT INTO tblCashPayment " +
                                "(PaymentVoucherNo, ServiceProviderID, PaymentDate, Amount, " +
                                 "Remark, UserID) " +
                            "VALUES " +
                                "(@PaymentVoucherNo, @ServiceProviderID, @PaymentDate, @Amount, " +
                                 "@Remark, @UserID)";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@PaymentVoucherNo", objCashPayment.PaymentVoucherNo);
                        myCommand.Parameters.AddWithValue("@ServiceProviderID", objCashPayment.ServiceProviderID);
                        myCommand.Parameters.AddWithValue("@PaymentDate", objCashPayment.PaymentDate);
                        myCommand.Parameters.AddWithValue("@Amount", objCashPayment.Amount);
                        myCommand.Parameters.AddWithValue("@Remark", objCashPayment.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objCashPayment.UserID);

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
                return new JsonResult("No Record Added, Service Provider ID Not Found!!");
        }

        [HttpPut]
        public JsonResult Put(clsCashPayment objCashPayment)
        {
            if (myServiceProvider.ServiceProviderIDFound(objCashPayment.ServiceProviderID))
            {
                string strSQL = "UPDATE tblCashPayment SET " +
                                "ServiceProviderID = @ServiceProviderID, " +
                                "PaymentDate = @PaymentDate, " +
                                "Amount = @Amount, " +
                                "Remark = @Remark, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE PaymentVoucherNo = @PaymentVoucherNo";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@PaymentVoucherNo", objCashPayment.PaymentVoucherNo);
                        myCommand.Parameters.AddWithValue("@ServiceProviderID", objCashPayment.ServiceProviderID);
                        myCommand.Parameters.AddWithValue("@PaymentDate", objCashPayment.PaymentDate);
                        myCommand.Parameters.AddWithValue("@Amount", objCashPayment.Amount);
                        myCommand.Parameters.AddWithValue("@Remark", objCashPayment.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objCashPayment.UserID);

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
                return new JsonResult("No Record Updated, Service Provider ID Not Found!!");
        }

        [HttpDelete("{_PaymentVoucherNo}")]
        public JsonResult Delete(string _PaymentVoucherNo)
        {
            string strSQL = "DELETE FROM tblCashPayment WHERE PaymentVoucherNo = @PaymentVoucherNo";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@PaymentVoucherNo", _PaymentVoucherNo);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }


        public int FindNumberOfCashPayment()
        {
            return myValidator.FindRecordCount("tblCashPayment", "PaymentVoucherNo");
        }

    }
}
