using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MvcCookieAuthSample.Models;

namespace MvcCookieAuthSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public IActionResult Index()
        {
            ServiceCollection service = new ServiceCollection();
            var ctx = service.BuildServiceProvider().GetService<IHttpContextAccessor>();

            var test = _contextAccessor.HttpContext;
            if (HttpContext==ctx)
            {
                Trace.WriteLine("相同");
            }
            if (HttpContext == test)
            {
                Trace.WriteLine("相同");
            }
            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
