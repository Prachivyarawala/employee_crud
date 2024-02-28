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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepositories _employeeRepositories;

        public EmployeeController(IEmployeeRepositories employeeRepositories)
        {
            _employeeRepositories = employeeRepositories;
        }
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            var emp = _employeeRepositories.FetchoneEmployee();
            return View(emp);
        }

        [HttpGet]
        public IActionResult Addemp()
        {
            var department = _employeeRepositories.GetAllDepartments();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname");

            return View();
        }

        // [HttpPost]
        // public IActionResult Addemp(Employee employee, IFormFile file)
        // {
        //     // string department = Request.Form["c_dept_id"].ToString();
        //     // employee.c_dept_id = department;

        //     var shift = Request.Form["c_shift"].ToList();
        //     employee.c_shift = string.Join(",", shift);




        //     var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        //     if (!Directory.Exists(folderPath))
        //     {
        //         Directory.CreateDirectory(folderPath);
        //     }

        //     var fileName = Path.GetFileName(file.FileName);
        //     var filePath = Path.Combine(folderPath, fileName);

        //     using (var stream = new FileStream(filePath, FileMode.Create))
        //     {
        //         file.CopyTo(stream);
        //     }

        //     var imageUrl = Path.Combine("/images", fileName);
        //     employee.c_image = imageUrl;


        //     _employeeRepositories.addemp(employee);
        //     return RedirectToAction("Index");
        // }

        [HttpPost]
        public IActionResult Addemp(Employee employee, IFormFile file)
        {
            var shift = Request.Form["c_shift"].ToList();
            employee.c_shift = string.Join(", ", shift);

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

                // Generate unique file name
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

        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            var department = _employeeRepositories.GetAllDepartments();
            var emp = _employeeRepositories.FetchoneEmployee();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname", emp.c_deptname.c_deptid);

            return View(emp);
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
        public IActionResult DeleteEmployee(int id)
        {
            _employeeRepositories.DeletetEmployee(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}