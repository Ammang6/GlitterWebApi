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
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT CustomerID, CustomerName, CustomerType, ContactPersons, " +
                            "Address, Telephone1, Telephone2, Mobile, Email, VATRegNo, TINNo " +
                            "FROM tblCustomer ORDER BY CustomerName";
            DataTable tblCustomer = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readCustomer;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readCustomer = myCommand.ExecuteReader();
                    tblCustomer.Load(readCustomer);
                    readCustomer.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblCustomer);
        }

        [HttpPost]
        public JsonResult Post(clsCustomer objCustomer)
        {
            string strSQL = "INSERT INTO tblCustomer " +
                                "(CustomerName, CustomerType, ContactPersons, Address, " +
                                 "Telephone1, Telephone2, Mobile, Email, VATRegNo, TINNo, UserID) " +
                            "VALUES " +
                                "(@CustomerName, @CustomerType, @ContactPersons, @Address, " +
                                 "@Telephone1, @Telephone2, @Mobile, @Email, @VATRegNo, @TINNo, @UserID)";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CustomerName", objCustomer.CustomerName);
                    myCommand.Parameters.AddWithValue("@CustomerType", objCustomer.CustomerType);
                    myCommand.Parameters.AddWithValue("@ContactPersons", objCustomer.ContactPersons);
                    myCommand.Parameters.AddWithValue("@Address", objCustomer.Address);
                    myCommand.Parameters.AddWithValue("@Telephone1", objCustomer.Telephone1);
                    myCommand.Parameters.AddWithValue("@Telephone2", objCustomer.Telephone2);
                    myCommand.Parameters.AddWithValue("@Mobile", objCustomer.Mobile);
                    myCommand.Parameters.AddWithValue("@Email", objCustomer.Email);
                    myCommand.Parameters.AddWithValue("@VATRegNo", objCustomer.VATRegNo);
                    myCommand.Parameters.AddWithValue("@TINNo", objCustomer.TINNo);
                    myCommand.Parameters.AddWithValue("@UserID", objCustomer.UserID);

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
        public JsonResult Put(clsCustomer objCustomer)
        {
            string strSQL = "UPDATE tblCustomer SET " +
                                "CustomerName = @CustomerName, " +
                                "CustomerType = @CustomerType, " +
                                "ContactPersons = @ContactPersons, " +
                                "Address = @Address, " +
                                "Telephone1 = @Telephone1, " +
                                "Telephone2 = @Telephone2, " +
                                "Mobile = @Mobile, " +
                                "Email = @Email, " +
                                "VATRegNo = @VATRegNo, " +
                                "TINNo = @TINNo, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE CustomerID = @CustomerID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CustomerID", objCustomer.CustomerID);
                    myCommand.Parameters.AddWithValue("@CustomerName", objCustomer.CustomerName);
                    myCommand.Parameters.AddWithValue("@CustomerType", objCustomer.CustomerType);
                    myCommand.Parameters.AddWithValue("@ContactPersons", objCustomer.ContactPersons);
                    myCommand.Parameters.AddWithValue("@Address", objCustomer.Address);
                    myCommand.Parameters.AddWithValue("@Telephone1", objCustomer.Telephone1);
                    myCommand.Parameters.AddWithValue("@Telephone2", objCustomer.Telephone2);
                    myCommand.Parameters.AddWithValue("@Mobile", objCustomer.Mobile);
                    myCommand.Parameters.AddWithValue("@Email", objCustomer.Email);
                    myCommand.Parameters.AddWithValue("@VATRegNo", objCustomer.VATRegNo);
                    myCommand.Parameters.AddWithValue("@TINNo", objCustomer.TINNo);
                    myCommand.Parameters.AddWithValue("@UserID", objCustomer.UserID);

                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Updated Successfully");
            else
                return new JsonResult("No Record Updated!!");
        }

        [HttpDelete("{_CustomerID}")]
        public JsonResult Delete(int _CustomerID)
        {
            string strSQL = "DELETE FROM tblCustomer WHERE CustomerID = @CustomerID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CustomerID", _CustomerID);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }

        public Boolean CustomerIDFound(int _CustomerID)
        {
            return myValidator.FindInTable("tblCustomer", "CustomerID", _CustomerID);
        }

        public int FindNumberOfCustomers()
        {
            return myValidator.FindRecordCount("tblCustomer", "CustomerID");
        }

    }
}
