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
    //[Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepositories _employeeRepositories;

        public EmployeeController(IEmployeeRepositories employeeRepositories)
        {
            _employeeRepositories = employeeRepositories;
        }
        public IActionResult Index()
        {
            var emp = _employeeRepositories.FetchoneEmployee();
            return View(emp);
        }

        [HttpGet]
        public IActionResult Addemp()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Addemp(Employee employee)
        {
            _employeeRepositories.addemp(employee);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}