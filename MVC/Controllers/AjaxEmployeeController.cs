using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using API.Repositories;
using API.Models;

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class AjaxEmployeeController : Controller
    {
        private readonly IEmployeeRepositories _employeeRepositories;
        private readonly ILogger<AjaxEmployeeController> _logger;

        public AjaxEmployeeController(IEmployeeRepositories employeeRepositories, ILogger<AjaxEmployeeController> logger)
        {
            _employeeRepositories = employeeRepositories;
            _logger = logger;
        }
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

        public IActionResult GetEmployee()
        {
            Console.WriteLine("controller ... " + HttpContext.Session.GetInt32("userid"));
            var allEmp = _employeeRepositories.FetchoneEmployee();
            return Json(allEmp);
        }

        [HttpGet]
        public IActionResult Addemp()
        {
            var department = _employeeRepositories.GetAllDepartments();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname");

            return View();
        }



        [HttpPost]
        public IActionResult Addemp(Employee employee, IFormFile file)
        {
            var shift = Request.Form["c_shift"].ToList();
            employee.c_shift = string.Join(",", shift);

            var department = _employeeRepositories.GetAllDepartments();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname");

            // Check if a file is uploaded
            if (file != null && file.Length > 0)
            {
                // Create folder if not exists
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Set the image URL in the employee object
                var imageUrl = Path.Combine("/images", fileName);
                employee.c_image = imageUrl;
            }

            _employeeRepositories.addemp(employee);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee, IFormFile? file = null)
        {

            var department = _employeeRepositories.GetAllDepartments();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname");

            if (file != null && file.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var imageUrl = Path.Combine("/images", fileName);
                employee.c_image = imageUrl;
            }
            Console.WriteLine("call add method");
            _employeeRepositories.addemp(employee);

            return Ok();
        }



        [HttpPost]
        public IActionResult UpdateEmployee(Employee emp, IFormFile? file = null)
        {
            var existingEmp = _employeeRepositories.FetchoneEmployee();
            if (existingEmp == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                emp.c_image = existingEmp.c_image;
            }
            else
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, file.FileName);
                var fileName = Path.GetFileName(file.FileName);

                if (System.IO.File.Exists(filePath))
                {
                    fileName = Guid.NewGuid().ToString() + "_" + fileName;
                    filePath = Path.Combine(folderPath, fileName);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var imageUrl = Path.Combine("/images", fileName);
                emp.c_image = imageUrl;
            }
            var shift = Request.Form["c_shift"].ToList();
            emp.c_shift = string.Join(", ", shift);
            if (_employeeRepositories.UpdateEmployee(emp))
            {
                return Ok();
            }

            return BadRequest(new { success = false, message = "Failed to update city" });
        }


        public IActionResult deleteEmployee(int id)
        {
            _employeeRepositories.DeletetEmployee(id);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}