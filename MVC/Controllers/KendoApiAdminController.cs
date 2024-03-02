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
    public class KendoApiAdminController : Controller
    {
        private readonly ILogger<KendoApiAdminController> _logger;
        private readonly IAdminRepositories _Adminripo;


        public KendoApiAdminController(ILogger<KendoApiAdminController> logger, IAdminRepositories Adminripo)
        {
            _logger = logger;
            _Adminripo = Adminripo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // public IActionResult GetAllEmployee()
        // {
        //     var allEmp = _Adminripo.getAllEmployee();
        //     return Json(allEmp);
        // }
        // public IActionResult GetAllDept()
        // {
        //     var GetAllDept = _Adminripo.GetAllDepartments();
        //     return Json(GetAllDept);
        // }
        // public IActionResult fetchEmpData(int id)
        // {
        //     var data = _Adminripo.FetchByEmpid(id);
        //     return Json(data);
        // }



        // [HttpPost]
        // public IActionResult EditEmployee(AdminEmployee employee)
        // {
        //     try
        //     {
        //         if (_Adminripo.UpdateEmployee(employee))
        //         {
        //             return Ok();
        //         }

        //         return BadRequest(new { success = false, message = "Failed to update city" });
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex);
        //         return Json(new { success = false, message = "An error occurred while updating employee." });
        //     }
        // }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}