using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class KendoCompoAdminController : Controller
    {
        private readonly ILogger<KendoCompoAdminController> _logger;

        public KendoCompoAdminController(ILogger<KendoCompoAdminController> logger)
        {
            _logger = logger;
        }

       [Produces("application/json")]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null || username != "admin")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}