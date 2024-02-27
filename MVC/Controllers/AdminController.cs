using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepositories _Adminripo;


        public AdminController(ILogger<AdminController> logger, IAdminRepositories Adminripo)
        {
            _logger = logger;
            _Adminripo = Adminripo;
        }

        public IActionResult Index()
        {
            var allemp =_Adminripo.getAllEmployee();
            return View(allemp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}