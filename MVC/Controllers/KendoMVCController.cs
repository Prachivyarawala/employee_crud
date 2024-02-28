using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class KendoMVCController : Controller
    {
        private readonly IAdminRepositories _Adminrepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public KendoMVCController(IAdminRepositories Adminrepo, IWebHostEnvironment webHostEnvironment)
        {
            _Adminrepo = Adminrepo;
            _webHostEnvironment = webHostEnvironment;
        }

        [Produces("application/json")]
        public IActionResult Index()
        {
            return View();
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
            var dept = _Adminrepo.GetAllDepartments();
            ViewBag.dept = new SelectList(dept, "c_dept_id", "c_deptname");
            return View(employee);
        }


        [HttpPost]
        public IActionResult Update(AdminEmployee employee)
        {
            Console.WriteLine("call : " + employee.c_dept_id);
            if (_Adminrepo.UpdateEmployee(employee))
            {
                return Json(new { success = true, message = "Successfully Added", newEmployeeId = employee.c_empid });
            }
            else
            {
                return Json(new { success = false, message = "Not Inserted!!" });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}