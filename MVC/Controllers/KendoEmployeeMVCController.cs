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
        private readonly IAdminRepositories _Adminrepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public KendoEmployeeMVCController(IAdminRepositories Adminrepo, IWebHostEnvironment webHostEnvironment)
        {
            _Adminrepo = Adminrepo;
            _webHostEnvironment = webHostEnvironment;
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
            ViewBag.IsAuthenticated = true;
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
            var dept = _Adminrepo.GetAllDepartments();

            return Json(dept);
        }


        




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}