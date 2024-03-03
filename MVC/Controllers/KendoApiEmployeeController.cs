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
    public class KendoApiEmployeeController : Controller
    {
        private readonly ILogger<KendoApiEmployeeController> _logger;
        private readonly IEmployeeRepositories _empripo;


        public KendoApiEmployeeController(ILogger<KendoApiEmployeeController> logger, IEmployeeRepositories empripo)
        {
            _logger = logger;
            _empripo = empripo;
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