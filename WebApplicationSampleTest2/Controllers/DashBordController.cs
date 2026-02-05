using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSampleTest2.Controllers
{
    public class DashBordController : Controller
    {
      
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return View();
        }

        public IActionResult Dash()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return View();
        }
    }
}
