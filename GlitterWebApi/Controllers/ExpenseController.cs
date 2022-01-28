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
    public class ExpenseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        private ExpenseListController myExpenseList;
        private OperationController myOperation;
        private ServiceProviderController myServiceProvider;

        public ExpenseController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator =new Validator(configuration);

            myExpenseList = new ExpenseListController(configuration);
            myOperation = new OperationController(configuration);
            myServiceProvider = new ServiceProviderController(configuration);
        }


        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT tblExpense.intAutoID, tblExpense.ExpenseID, tblExpenseList.ExpenseDescription, "+
                            "tblExpense.OperationNumber, tblOperation.CustomerID, tblCustomer.CustomerName, " +
                            "tblExpense.ServiceProviderID, tblServiceProvider.ServiceProviderName, tblExpense.ExpenseDate, " +
                            "tblExpense.Amount, tblExpense.Remark FROM tblExpense INNER JOIN tblExpenseList " +
                            "ON tblExpense.ExpenseID = tblExpenseList.ExpenseID INNER JOIN tblOperation ON " +
                            "tblExpense.OperationNumber = tblOperation.OperationNumber INNER JOIN tblCustomer " +
                            "ON tblOperation.CustomerID = tblCustomer.CustomerID INNER JOIN tblServiceProvider " +
                            "ON tblExpense.ServiceProviderID = tblServiceProvider.ServiceProviderID ORDER BY intAutoID";
            DataTable tblExpense = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readExpense;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readExpense = myCommand.ExecuteReader();
                    tblExpense.Load(readExpense);
                    readExpense.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblExpense);
        }


        [HttpPost]
        public JsonResult Post(clsExpense objExpense)
        {
            if (myExpenseList.ExpenseIDFound(objExpense.ExpenseID) == false)
            {
                return new JsonResult("No Record Added, Expense ID Not Found!!");
            }
            else if (myOperation.OperationNumberFound(objExpense.OperationNumber) == false)
            {
                return new JsonResult("No Record Added, Operation Number Not Found!!");
            }
            else if (myServiceProvider.ServiceProviderIDFound(objExpense.ServiceProviderID) == false)
            {
                return new JsonResult("No Record Added, Service Provider ID Not Found!!");
            }
            else
            {
                string strSQL = "INSERT INTO tblExpense " +
                                "(ExpenseID, OperationNumber, ServiceProviderID, ExpenseDate, Amount, " +
                                 "Remark, UserID) " +
                            "VALUES " +
                                "(@ExpenseID, @OperationNumber, @ServiceProviderID, @ExpenseDate, @Amount, " +
                                 "@Remark, @UserID)";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@ExpenseID", objExpense.ExpenseID);
                        myCommand.Parameters.AddWithValue("@OperationNumber", objExpense.OperationNumber);
                        myCommand.Parameters.AddWithValue("@ServiceProviderID", objExpense.ServiceProviderID);
                        myCommand.Parameters.AddWithValue("@ExpenseDate", objExpense.ExpenseDate);
                        myCommand.Parameters.AddWithValue("@Amount", objExpense.Amount);
                        myCommand.Parameters.AddWithValue("@Remark", objExpense.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objExpense.UserID);

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
        public JsonResult Put(clsExpense objExpense)
        {
            if (myExpenseList.ExpenseIDFound(objExpense.ExpenseID) == false)
            {
                return new JsonResult("No Record Added, Expense ID Not Found!!");
            }
            else if (myOperation.OperationNumberFound(objExpense.OperationNumber) == false)
            {
                return new JsonResult("No Record Added, Operation Number Not Found!!");
            }
            else if (myServiceProvider.ServiceProviderIDFound(objExpense.ServiceProviderID) == false)
            {
                return new JsonResult("No Record Added, Service Provider ID Not Found!!");
            }
            else
            {
                string strSQL = "UPDATE tblExpense SET " +
                                "ExpenseID = @ExpenseID, " +
                                "OperationNumber = @OperationNumber, " +
                                "ServiceProviderID = @ServiceProviderID, " +
                                "ExpenseDate = @ExpenseDate, " +
                                "Amount = @Amount, " +
                                "Remark = @Remark, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE intAutoID = @intAutoID";
                int AffectedRecords = 0;
                string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@intAutoID", objExpense.AutoID);
                        myCommand.Parameters.AddWithValue("@ExpenseID", objExpense.ExpenseID);
                        myCommand.Parameters.AddWithValue("@OperationNumber", objExpense.OperationNumber);
                        myCommand.Parameters.AddWithValue("@ServiceProviderID", objExpense.ServiceProviderID);
                        myCommand.Parameters.AddWithValue("@ExpenseDate", objExpense.ExpenseDate);
                        myCommand.Parameters.AddWithValue("@Amount", objExpense.Amount);
                        myCommand.Parameters.AddWithValue("@Remark", objExpense.Remark);
                        myCommand.Parameters.AddWithValue("@UserID", objExpense.UserID);

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


        [HttpDelete("{_intAutoID}")]
        public JsonResult Delete(int _intAutoID)
        {
            string strSQL = "DELETE FROM tblExpense WHERE intAutoID = @intAutoID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@intAutoID", _intAutoID);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }


        public int FindNumberOfExpense()
        {
            return myValidator.FindRecordCount("tblExpense", "intAutoID");
        }

    }
}
