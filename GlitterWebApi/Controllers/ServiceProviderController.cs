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
    public class ServiceProviderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        public ServiceProviderController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);
        }
        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT ServiceProviderID, ServiceProviderName, ServiceType, ContactPersons, " +
                            "Address, Telephone1, Telephone2, Mobile, Email, VATRegNo, TINNo " +
                            "FROM tblServiceProvider ORDER BY ServiceProviderName";
            DataTable tblServiceProvider = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readServiceProvider;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readServiceProvider = myCommand.ExecuteReader();
                    tblServiceProvider.Load(readServiceProvider);
                    readServiceProvider.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblServiceProvider);
        }

        [HttpPost]
        public JsonResult Post(clsServiceProvider objServiceProvider)
        {
            string strSQL = "INSERT INTO tblServiceProvider " +
                                "(ServiceProviderName, ServiceType, ContactPersons, Address, " +
                                 "Telephone1, Telephone2, Mobile, Email, VATRegNo, TINNo, UserID) " +
                            "VALUES " +
                                "(@ServiceProviderName, @ServiceType, @ContactPersons, @Address, " +
                                 "@Telephone1, @Telephone2, @Mobile, @Email, @VATRegNo, @TINNo, @UserID)";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ServiceProviderName", objServiceProvider.ServiceProviderName);
                    myCommand.Parameters.AddWithValue("@ServiceType", objServiceProvider.ServiceType);
                    myCommand.Parameters.AddWithValue("@ContactPersons", objServiceProvider.ContactPersons);
                    myCommand.Parameters.AddWithValue("@Address", objServiceProvider.Address);
                    myCommand.Parameters.AddWithValue("@Telephone1", objServiceProvider.Telephone1);
                    myCommand.Parameters.AddWithValue("@Telephone2", objServiceProvider.Telephone2);
                    myCommand.Parameters.AddWithValue("@Mobile", objServiceProvider.Mobile);
                    myCommand.Parameters.AddWithValue("@Email", objServiceProvider.Email);
                    myCommand.Parameters.AddWithValue("@VATRegNo", objServiceProvider.VATRegNo);
                    myCommand.Parameters.AddWithValue("@TINNo", objServiceProvider.TINNo);
                    myCommand.Parameters.AddWithValue("@UserID", objServiceProvider.UserID);

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
        public JsonResult Put(clsServiceProvider objServiceProvider)
        {
            string strSQL = "UPDATE tblServiceProvider SET " +
                                "ServiceProviderName = @ServiceProviderName, " +
                                "ServiceType = @ServiceType, " +
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
                                "WHERE ServiceProviderID = @ServiceProviderID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ServiceProviderID", objServiceProvider.ServiceProviderID);
                    myCommand.Parameters.AddWithValue("@ServiceProviderName", objServiceProvider.ServiceProviderName);
                    myCommand.Parameters.AddWithValue("@ServiceType ", objServiceProvider.ServiceType);
                    myCommand.Parameters.AddWithValue("@ContactPersons", objServiceProvider.ContactPersons);
                    myCommand.Parameters.AddWithValue("@Address", objServiceProvider.Address);
                    myCommand.Parameters.AddWithValue("@Telephone1", objServiceProvider.Telephone1);
                    myCommand.Parameters.AddWithValue("@Telephone2", objServiceProvider.Telephone2);
                    myCommand.Parameters.AddWithValue("@Mobile", objServiceProvider.Mobile);
                    myCommand.Parameters.AddWithValue("@Email", objServiceProvider.Email);
                    myCommand.Parameters.AddWithValue("@VATRegNo", objServiceProvider.VATRegNo);
                    myCommand.Parameters.AddWithValue("@TINNo", objServiceProvider.TINNo);
                    myCommand.Parameters.AddWithValue("@UserID", objServiceProvider.UserID);

                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Updated Successfully");
            else
                return new JsonResult("No Record Updated!!");
        }

        [HttpDelete("{_ServiceProviderID}")]
        public JsonResult Delete(int _ServiceProviderID)
        {
            string strSQL = "DELETE FROM tblServiceProvider WHERE ServiceProviderID = @ServiceProviderID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ServiceProviderID", _ServiceProviderID);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }

        public Boolean ServiceProviderIDFound(int _ServiceProviderID)
        {
            return myValidator.FindInTable("tblServiceProvider", "ServiceProviderID", _ServiceProviderID);
        }

        public int FindNumberOfServiceProvider()
        {
            return myValidator.FindRecordCount("tblServiceProvider", "ServiceProviderID");
        }

    }
}
