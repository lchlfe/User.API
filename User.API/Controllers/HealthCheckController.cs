using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("HealthCheck")]
    public class HealthCheckController : Controller
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpHead]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}