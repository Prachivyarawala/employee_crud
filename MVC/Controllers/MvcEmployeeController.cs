using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
namespace MVC.Controllers
{
    public class MvcEmployeeController : Controller
    {
        private readonly ILogger<MvcEmployeeController> _logger;
        private readonly IEmployeeRepositories _emprepo;

        public MvcEmployeeController(ILogger<MvcEmployeeController> logger, IEmployeeRepositories emprepo)
        {
            _logger = logger;
            _emprepo = emprepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsAuthenticated = true;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}