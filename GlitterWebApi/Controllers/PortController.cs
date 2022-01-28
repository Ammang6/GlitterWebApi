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
    public class PortController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        public PortController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT PortID, PortName, Country FROM tblPort " +
                            "ORDER BY PortName";
            DataTable tblPort = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readPort;
            using(SqlConnection myConn=new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using(SqlCommand myCommand=new SqlCommand(strSQL,myConn))
                {
                    readPort = myCommand.ExecuteReader();
                    tblPort.Load(readPort);
                    readPort.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblPort);
        }

        [HttpPost]
        public JsonResult Post(clsPort objPort)
        {
            string strSQL = "INSERT INTO tblPort " +
                                "(PortName, Country, UserID) " +
                            "VALUES " +
                                "(@PortName, @Country, @UserID)";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@PortName", objPort.PortName);
                    myCommand.Parameters.AddWithValue("@Country", objPort.Country);
                    myCommand.Parameters.AddWithValue("@UserID", objPort.UserID);

                    AffectedRecords= myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Added Successfully");
            else
                return new JsonResult("No Record Added!!");
        }

        [HttpPut]
        public JsonResult Put(clsPort objPort)
        {
            string strSQL = "UPDATE tblPort SET " +
                                "PortName = @PortName, " +
                                "Country = @Country, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE PortID = @PortID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@PortID", objPort.PortID);
                    myCommand.Parameters.AddWithValue("@PortName", objPort.PortName);
                    myCommand.Parameters.AddWithValue("@Country", objPort.Country);
                    myCommand.Parameters.AddWithValue("@UserID", objPort.UserID);

                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Updated Successfully");
            else
                return new JsonResult("No Record Updated!!");
        }

        [HttpDelete("{_PortID}")]
        public JsonResult Delete(int _PortID)
        {
            string strSQL = "DELETE FROM tblPort WHERE PortID = @PortID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@PortID", _PortID);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }

        public Boolean PortIDFound(int _PortID)
        {
            return myValidator.FindInTable("tblPort", "PortID", _PortID);
        }

        public int FindNumberOfPorts()
        {
            return myValidator.FindRecordCount("tblPort", "PortID");
        }
    }
}
