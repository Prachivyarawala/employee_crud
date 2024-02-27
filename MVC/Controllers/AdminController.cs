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
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminRepositories _Adminripo;


        public AdminController(ILogger<AdminController> logger, IAdminRepositories Adminripo)
        {
            _logger = logger;
            _Adminripo = Adminripo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            
            var allemp = _Adminripo.getAllEmployee();
            return View(allemp);
        }
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var employee = _Adminripo.FetchByEmpid(id);
            if (employee == null)
            {
                return NotFound();
            }
            var departments = _Adminripo.GetAllDepartments();
            ViewBag.Departments = new SelectList(departments, "c_deptid", "c_deptname", employee.c_deptname.c_deptid);
            return View(employee);
        }
        [HttpPost]
        public IActionResult EditEmployee(AdminEmployee employee)
        {
            try
            {
                if (_Adminripo.UpdateEmployee(employee))
                {
                    Console.WriteLine("updated");
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View(employee);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            // If the execution reaches here, there was an error or validation issue, so return the view with appropriate data
            var departments = _Adminripo.GetAllDepartments();
            ViewBag.Departments = new SelectList(departments, "c_deptid", "c_deptname", employee.c_dept_id);
            return View(employee);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}