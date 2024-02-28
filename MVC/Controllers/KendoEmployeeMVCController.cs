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
    public class KendoEmployeeMVCController : Controller
    {
        // private readonly IAdminRepositories _Adminrepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmployeeRepositories _employeeRepositories;
        public KendoEmployeeMVCController(IEmployeeRepositories employeeRepositories, IWebHostEnvironment webHostEnvironment)
        {
            // _Adminrepo = Adminrepo;
            _webHostEnvironment = webHostEnvironment;
            _employeeRepositories = employeeRepositories;
        }

        [Produces("application/json")]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.IsAuthenticated = false;
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        // public IActionResult GetEmployee()
        // {
        //     Console.WriteLine("controller ... " + HttpContext.Session.GetInt32("userid"));
        //     var allEmp = _employeeRepositories.FetchoneEmployee();
        //     return Json(allEmp);
        // }

        public IActionResult GetAlldept()
        {
            var dept = _employeeRepositories.GetAllDepartments();

            return Json(dept);
        }


        public IActionResult getemployee()
        {
            Console.WriteLine("controller ... " + HttpContext.Session.GetInt32("userid"));

            var emp = _employeeRepositories.FetchoneEmployee();
            Console.WriteLine("name" + emp.c_empname);
            Console.WriteLine("gender : "+ emp.c_enpgender);
            Console.WriteLine("image : "+ emp.c_image);
            return Json(emp);
        }
         [HttpPost]
    public IActionResult Add( Employee employee)
    {
       
            _employeeRepositories.addemp(employee);
            return Json(employee);
       
    }

    [HttpPost]
    public IActionResult Edit( Employee employee)
    {
        
            _employeeRepositories.UpdateEmployee(employee);
            return Json(employee);
       
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
       
            _employeeRepositories.DeletetEmployee(id);
            return Json(id);
       
    }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}