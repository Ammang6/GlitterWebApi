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
    public class Validator : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Validator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Boolean FindInTable(string pTableName, string pFieldName, int pFieldValue)
        {
            Boolean _Result = false;

            string strSQL = "SELECT " + pFieldName + " FROM " + pTableName + " WHERE " + pFieldName + " = " + pFieldValue ;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader tempReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    tempReader = myCommand.ExecuteReader();

                    _Result = tempReader.HasRows;
                    
                    tempReader.Close();
                    myConn.Close();
                }
            }
            return _Result;
        }

        public Boolean FindInTable(string pTableName, string pFieldName, string pFieldValue)
        {
            Boolean _Result = false;

            string strSQL = "SELECT " + pFieldName + " FROM " + pTableName + " WHERE " + 
                             pFieldName + " = '" +  pFieldValue.Replace("'","''") + "'";
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader tempReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    tempReader = myCommand.ExecuteReader();

                    _Result = tempReader.HasRows;

                    tempReader.Close();
                    myConn.Close();
                }
            }
            return _Result;
        }

        public int FindRecordCount(string pTableName, string pFieldName)
        {
            int _Result = 0;

            string strSQL = "SELECT COUNT(" + pFieldName + ") AS " + pFieldName + " FROM " + pTableName;
            string sqlDataSource = _configuration.GetConnectionString("GlitterDBConnection");
            SqlDataReader tempReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(strSQL, myConn))
                {
                    tempReader = myCommand.ExecuteReader();

                    if (tempReader.Read())
                        _Result = (int)tempReader[pFieldName];

                    tempReader.Close();
                    myConn.Close();
                }
            }
            return _Result;
        }
    }
}
