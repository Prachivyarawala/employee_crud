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
    public class KendoCompoAdminAjaxController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
         private readonly IAdminRepositories _Adminripo;
        public KendoCompoAdminAjaxController(IAdminRepositories Adminrepo, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _Adminripo  = Adminrepo;
        }

        [Produces("application/json")]
        [HttpGet]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null || username != "admin")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        public IActionResult GetAllEmployee()
        {
            var allEmp = _Adminripo.getAllEmployee();
            return Json(allEmp);
        }
        public IActionResult GetAllDept()
        {
            var GetAllDept = _Adminripo.GetAllDepartments();
            return Json(GetAllDept);
        }
        public IActionResult fetchEmpData(int id)
        {
            var data = _Adminripo.FetchByEmpid(id);
            return Json(data);
        }



        [HttpPost]
        public IActionResult EditEmployee(AdminEmployee employee)
        {
            try
            {
                if (_Adminripo.UpdateEmployee(employee))
                {
                    return Ok();
                }

                return BadRequest(new { success = false, message = "Failed to update city" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { success = false, message = "An error occurred while updating employee." });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}