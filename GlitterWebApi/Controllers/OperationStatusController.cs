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
    public class OperationStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Validator myValidator;

        public OperationStatusController(IConfiguration configuration)
        {
            _configuration = configuration;
            myValidator = new Validator(configuration);
        }
    }
}
