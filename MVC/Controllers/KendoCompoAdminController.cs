using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models;
using API.Repositories;

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class KendoCompoAdminController : Controller
    {
        private readonly ILogger<KendoCompoAdminController> _logger;
        private readonly IAdminRepositories _Adminrepo;

        public KendoCompoAdminController(ILogger<KendoCompoAdminController> logger,  IAdminRepositories Adminrepo)
        {
            _logger = logger;
            _Adminrepo = Adminrepo;
        }

       [Produces("application/json")]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null || username != "admin")
            {
                return RedirectToAction("Login", "User");
            }
            var employee = _Adminrepo.getAllEmployee();
            return View(employee);
        }

        public IActionResult GetAlldept()
        {
            var dept = _Adminrepo.GetAllDepartments();

            return Json(dept);
        }
        public IActionResult getAllEmployee()
        {
            var employee = _Adminrepo.getAllEmployee();
            return Json(employee);
        }

        public IActionResult Update(int id)
        {
            AdminEmployee employee = _Adminrepo.FetchByEmpid(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            return View(employee);
        }

        [HttpPost]
        public IActionResult Update(AdminEmployee employee)
        {
            Console.WriteLine("call : " + employee.c_dept_id);
            var shift = Request.Form["c_shift"].ToList();
            employee.c_shift = string.Join(",", shift);
            if (_Adminrepo.UpdateEmployee(employee))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(employee);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}