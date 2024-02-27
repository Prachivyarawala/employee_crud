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

        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.IsAuthenticated = false;
                return RedirectToAction("Login", "User");
            }
            ViewBag.IsAuthenticated = true;
            var department = _emprepo.GetAllDepartments();
            var emp = _emprepo.FetchoneEmployee();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname",emp.c_deptname.c_deptid);

            return View(emp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}