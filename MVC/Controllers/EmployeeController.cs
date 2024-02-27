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

            // Add the employee to the repository
            _employeeRepositories.addemp(employee);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.IsAuthenticated = false;
                return RedirectToAction("Login", "User");
            }
            ViewBag.IsAuthenticated = true;
            var department = _employeeRepositories.GetAllDepartments();
            var emp = _employeeRepositories.FetchoneEmployee();
            ViewBag.department = new SelectList(department, "c_deptid", "c_deptname", emp.c_deptname.c_deptid);

            return View(emp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}