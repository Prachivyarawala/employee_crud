using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Repositories;
using API.Models;


namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepositories _emprepo;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepositories emprepo)
        {
            _logger = logger;
            _emprepo = emprepo;
        }

        public IActionResult Index()
        {
            var emp = _emprepo.FetchoneEmployee();
            return View(emp);
        }


        [HttpGet]
        public IActionResult AddEmployee()
        {
            _emprepo.
            return View(emp);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}