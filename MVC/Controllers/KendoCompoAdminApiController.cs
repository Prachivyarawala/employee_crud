using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using API.Models;


namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class KendoCompoAdminApiController : Controller
    {
        private readonly ILogger<KendoCompoAdminApiController> _logger;
        private readonly IAdminRepositories _Adminripo;


        public KendoCompoAdminApiController(ILogger<KendoCompoAdminApiController> logger, IAdminRepositories Adminripo)
        {
            _logger = logger;
            _Adminripo = Adminripo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}