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
    public class CashCollectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        private CustomerController myCustomer;

        public CashCollectionController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);

            myCustomer = new CustomerController(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT tblCashCollection.CRVNumber, tblCashCollection.CustomerID, tblCustomer.CustomerName, " +
                            "tblCashCollection.CollectionDate, tblCashCollection.Amount, " +
                            "tblCashCollection.Remark FROM tblCashCollection INNER JOIN tblCustomer ON " +
                            "tblCashCollection.CustomerID = tblCustomer.CustomerID ORDER BY CRVNumber";
            DataTable tblCashCollection = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readCash;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readCash = myCommand.ExecuteReader();
                    tblCashCollection.Load(readCash);
                    readCash.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblCashCollection);
        }

        [HttpPost]
        public JsonResult Post(clsCashCollection objCashCollection)
        {
            if (myCustomer.CustomerIDFound(objCashCollection.CustomerID))
            {
                string strSQL = "INSERT INTO tblCashCollection " +
                                "(CRVNumber, CustomerID, CollectionDate, Amount, " +
                                 "Remark, UserID) " +
                            "VALUES " +
                                "(@CRVNumber, @CustomerID, @CollectionDate, @Amount, " +
                                 "@Remark, @UserID)";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@CRVNumber", objCashCollection.CRVNumber);
                        myCommand.Parameters.AddWithValue("@CustomerID", objCashCollection.CustomerID);
                        myCommand.Parameters.AddWithValue("@CollectionDate", objCashCollection.CollectionDate);
                        myCommand.Parameters.AddWithValue("@Amount", objCashCollection.Amount);
                        myCommand.Parameters.AddWithValue("@Remark", objCashCollection.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objCashCollection.UserID);

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
                return new JsonResult("No Record Added, Customer ID Not Found!!");
        }

        [HttpPut]
        public JsonResult Put(clsCashCollection objCashCollection)
        {
            if (myCustomer.CustomerIDFound(objCashCollection.CustomerID))
            {
                string strSQL = "UPDATE tblCashCollection SET " +
                                "CustomerID = @CustomerID, " +
                                "CollectionDate = @CollectionDate, " +
                                "Amount = @Amount, " +
                                "Remark = @Remark, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE CRVNumber = @CRVNumber";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@CRVNumber", objCashCollection.CRVNumber);
                        myCommand.Parameters.AddWithValue("@CustomerID", objCashCollection.CustomerID);
                        myCommand.Parameters.AddWithValue("@CollectionDate", objCashCollection.CollectionDate);
                        myCommand.Parameters.AddWithValue("@Amount", objCashCollection.Amount);
                        myCommand.Parameters.AddWithValue("@Remark", objCashCollection.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objCashCollection.UserID);

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
                return new JsonResult("No Record Updated, Customer ID Not Found!!");
        }

        [HttpDelete("{_CRVNumber}")]
        public JsonResult Delete(string _CRVNumber)
        {
            string strSQL = "DELETE FROM tblCashCollection WHERE CRVNumber = @CRVNumber";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CRVNumber", _CRVNumber);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }


        public int FindNumberOfCashCollection()
        {
            return myValidator.FindRecordCount("tblCashCollection", "CRVNumber");
        }
    }
}
