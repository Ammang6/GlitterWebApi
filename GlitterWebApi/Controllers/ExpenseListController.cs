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
    public class ExpenseListController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        public ExpenseListController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT ExpenseID, ExpenseDescription FROM tblExpenseList " +
                            "ORDER BY ExpenseDescription";
            DataTable tblExpenseList = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readExpenseList;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readExpenseList = myCommand.ExecuteReader();
                    tblExpenseList.Load(readExpenseList);
                    readExpenseList.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblExpenseList);
        }

        [HttpPost]
        public JsonResult Post(clsExpenseList objExpenseList)
        {
            string strSQL = "INSERT INTO tblExpenseList " +
                                "(ExpenseDescription, UserID) " +
                            "VALUES " +
                                "(@ExpenseDescription, @UserID)";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ExpenseDescription", objExpenseList.ExpenseDescription);
                    myCommand.Parameters.AddWithValue("@UserID", objExpenseList.UserID);

                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Added Successfully");
            else
                return new JsonResult("No Record Added!!");
        }

        [HttpPut]
        public JsonResult Put(clsExpenseList objExpenseList)
        {
            string strSQL = "UPDATE tblExpenseList SET " +
                                "ExpenseDescription = @ExpenseDescription, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE ExpenseID = @ExpenseID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ExpenseID", objExpenseList.ExpenseID);
                    myCommand.Parameters.AddWithValue("@ExpenseDescription", objExpenseList.ExpenseDescription);
                    myCommand.Parameters.AddWithValue("@UserID", objExpenseList.UserID);

                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Updated Successfully");
            else
                return new JsonResult("No Record Updated!!");
        }

        [HttpDelete("{_ExpenseID}")]
        public JsonResult Delete(int _ExpenseID)
        {
            string strSQL = "DELETE FROM tblExpenseList WHERE ExpenseID = @ExpenseID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ExpenseID", _ExpenseID);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }

        public Boolean ExpenseIDFound(int _ExpenseID)
        {
            return myValidator.FindInTable("tblExpenseList", "ExpenseID", _ExpenseID);
        }

        public int FindNumberOfExpenseList()
        {
            return myValidator.FindRecordCount("tblExpenseList", "ExpenseID");
        }

    }
}
