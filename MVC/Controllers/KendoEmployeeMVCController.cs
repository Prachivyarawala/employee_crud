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
        // private readonly IAdminRepositories _employeeRepositories;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmployeeRepositories _employeeRepositories;
        public KendoEmployeeMVCController(IEmployeeRepositories employeeRepositories, IWebHostEnvironment webHostEnvironment)
        {
            // _employeeRepositories = Adminrepo;
            _webHostEnvironment = webHostEnvironment;
            _employeeRepositories = employeeRepositories;
        }
        [Produces("application/json")]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        public IActionResult GetAlldept()
        {
            var dept = _employeeRepositories.GetAllDepartments();

            return Json(dept);
        }
        public IActionResult getemployee()
        {
            Console.WriteLine("controller ... " + HttpContext.Session.GetInt32("userid"));

            var emp = _employeeRepositories.FetchoneEmployee();
            return Json(emp);
        }
        public IActionResult Update(int id)
        {
            Console.WriteLine("controller ... " + HttpContext.Session.GetInt32("userid"));
            Employee employee = _employeeRepositories.FetchoneEmployee();
            if (employee == null)
            {
                return NotFound();
            }
            var dept = _employeeRepositories.GetAllDepartments();
            ViewBag.dept = new SelectList(dept, "c_deptid", "c_deptname", employee.c_dept_id); // Pass the selected department ID
            return View(employee);
        }



        [HttpPost]
        public IActionResult Update(Employee employee, IFormFile? file = null)
        {
            Console.WriteLine("call : " + employee.c_dept_id);
            var existingEmp = _employeeRepositories.FetchoneEmployee();
            if (existingEmp == null)
            {
                return NotFound();
            }
            if (file == null || file.Length == 0)
            {
                employee.c_image = existingEmp.c_image;
            }
            else
            {
                var folderPath = @"..\MVC\wwwroot\images";

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
                employee.c_image = imageUrl;
            }

            if (_employeeRepositories.UpdateEmployee(employee))
            {
                return Json(new { success = true, message = "Successfully Added", newEmployeeId = employee.c_empid });
            }
            else
            {
                return Json(new { success = false, message = "Not Inserted!!" });
            }
        }
        [HttpPost]
        public IActionResult AddEmployee(Employee employee, IFormFile? file = null)
        {
            Console.WriteLine("call : " + employee.c_dept_id);
            if (file != null && file.Length > 0)
            {
                // Create folder if not exists
                var folderPath = @"..\MVC\wwwroot\images";
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

            if (_employeeRepositories.addemp(employee))
            {
                return Json(new { success = true, message = "Successfully Added", newEmployeeId = employee.c_empid });
            }
            else
            {
                return Json(new { success = false, message = "Not Inserted!!" });
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_employeeRepositories.DeletetEmployee(id))
                {
                    return Json(new { success = true, message = "Successfully Deleted" });
                }
                else
                {
                    return Json(new { success = false, message = "Deletion Failed" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting employee: " + ex.Message);
                return Json(new { success = false, message = "An error occurred while deleting employee" });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}