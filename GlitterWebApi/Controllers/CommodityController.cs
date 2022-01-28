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
    public class CommodityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        public CommodityController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string strSQL = "SELECT CommodityID, CommodityName, Category, HSCode " +
                            "FROM tblCommodity ORDER BY CommodityName";
            DataTable tblCommodity = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader readCommodity;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    readCommodity = myCommand.ExecuteReader();
                    tblCommodity.Load(readCommodity);
                    readCommodity.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(tblCommodity);
        }

        [HttpPost]
        public JsonResult Post(clsCommodity objCommodity)
        {
            string strSQL = "INSERT INTO tblCommodity " +
                                "(CommodityName, Category, HSCode, UserID) " +
                            "VALUES " +
                                "(@CommodityName, @Category, @HSCode, @UserID)";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CommodityName", objCommodity.CommodityName);
                    myCommand.Parameters.AddWithValue("@Category", objCommodity.Category);
                    myCommand.Parameters.AddWithValue("@HSCode", objCommodity.HSCode);
                    myCommand.Parameters.AddWithValue("@UserID", objCommodity.UserID);

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
        public JsonResult Put(clsCommodity objCommodity)
        {
            string strSQL = "UPDATE tblCommodity SET " +
                                "CommodityName = @CommodityName, " +
                                "Category = @Category, " +
                                "HSCode = @HSCode, " +
                                "UserID = @UserID, " +
                                "RecordDate = getdate() " +
                                "WHERE CommodityID = @CommodityID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CommodityID", objCommodity.CommodityID);
                    myCommand.Parameters.AddWithValue("@CommodityName", objCommodity.CommodityName);
                    myCommand.Parameters.AddWithValue("@Category", objCommodity.Category);
                    myCommand.Parameters.AddWithValue("@HSCode", objCommodity.HSCode);
                    myCommand.Parameters.AddWithValue("@UserID", objCommodity.UserID);

                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Updated Successfully");
            else
                return new JsonResult("No Record Updated!!");
        }

        [HttpDelete("{_CommodityID}")]
        public JsonResult Delete(int _CommodityID)
        {
            string strSQL = "DELETE FROM tblCommodity WHERE CommodityID = @CommodityID";
            int AffectedRecords = 0;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CommodityID", _CommodityID);
                    AffectedRecords = myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            if (AffectedRecords > 0)
                return new JsonResult(AffectedRecords + " Record Deleted Successfully");
            else
                return new JsonResult("No Record Deleted!!");
        }

        public Boolean CommodityIDFound(int _CommodityID)
        {
            return myValidator.FindInTable("tblCommodity", "CommodityID", _CommodityID);
        }


        public int FindNumberOfCommodity()
        {
            return myValidator.FindRecordCount("tblCommodity", "CommodityID");
        }

    }
}
