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
    public class KendoComponentAjaxEmpController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmployeeRepositories _employeeRepositories;
        public KendoComponentAjaxEmpController(IEmployeeRepositories employeeRepositories, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _employeeRepositories = employeeRepositories;
        }
        [Produces("application/json")]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "KendoUser");
            }
            var emp = _employeeRepositories.FetchoneEmployee();
            return View(emp);
        }
        public IActionResult getemployee()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "KendoUser");
            }
            Console.WriteLine("controller ... " + HttpContext.Session.GetInt32("userid"));

            var emp = _employeeRepositories.FetchoneEmployee();
            return Json(emp);
        }
        public IActionResult GetAllDept()
        {
            var GetAllDept = _employeeRepositories.GetAllDepartments();
            return Json(GetAllDept);
        }

        [HttpPost]
        public IActionResult Addemp(Employee employee)
        {
            var shift = Request.Form["c_shift"].ToList();
            employee.c_shift = string.Join(", ", shift);

            if (_employeeRepositories.addemp(employee))
            {
                return Ok();
            }
            else{
                return BadRequest(new { success = false, message = "Failed to update Employee" });
            }

        }

        [HttpPost]
        public IActionResult UpdateEmployee(Employee? emp=null)
        {
            var existingEmp = _employeeRepositories.FetchoneEmployee();
            if (existingEmp == null)
            {
                return NotFound();
            }

            if (emp.c_image == null || emp.c_image.Length == 0)
            {
                emp.c_image = existingEmp.c_image;
            }
            var shift = Request.Form["c_shift"].ToList();
            emp.c_shift = string.Join(", ", shift);
            if (_employeeRepositories.UpdateEmployee(emp))
            {
                return Ok();
            }

            return BadRequest(new { success = false, message = "Failed to update Employee" });
        }
        public IActionResult DeleteEmployee(int id)
        {
            _employeeRepositories.DeletetEmployee(id);
            return Ok();
        }
        [HttpPost]
        public IActionResult SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            var folderPath = @"..\MVC\wwwroot\images";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var imageUrl = Path.Combine("/images", file.FileName); // Assuming the URL to access the image is /images/{filename}
            return Ok(new { imageUrl });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}