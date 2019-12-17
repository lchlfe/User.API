using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("HealthCheck")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        [HttpHead]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}