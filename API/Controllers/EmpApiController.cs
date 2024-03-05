using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Models;

namespace API.Controllers
{

    [ApiController]
    //[Route("api/[controller]")]
    public class EmpApiController : ControllerBase
    {
        private readonly IEmployeeRepositories _employeeRepositories;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmpApiController(IEmployeeRepositories employeeRepositories, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepositories = employeeRepositories;
            _httpContextAccessor = httpContextAccessor;
        }



        [HttpGet("getemployee")]
        public IActionResult getemployee(int id)
        {
            HttpContext.Session.SetInt32("userid", id);
            var emp = _employeeRepositories.FetchoneEmployee();
            return Ok(emp);
        }

        [HttpPut("UpdateEmployee")]
        public IActionResult UpdateEmployee(int id, [FromForm] Employee? emp = null, IFormFile? file = null)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("userid" , emp.c_userid.GetValueOrDefault());
            var existingEmp = _employeeRepositories.FetchoneEmployee();
            Console.WriteLine("existing image: " + emp.c_image);
            if (existingEmp == null)
            {
                return NotFound();
            }

            // Console.WriteLine("empid : " + emp.c_empid);
            // Console.WriteLine("empname : " + emp.c_empname);
            // Console.WriteLine("emp gender : " + emp.c_enpgender);
            // Console.WriteLine("emp shift : " + emp.c_shift);
            // Console.WriteLine("emp dob : " + emp.c_dob);
            // Console.WriteLine("emp dept id : " + emp.c_dept_id);

            if (file == null || file.Length == 0)
            {
                Console.WriteLine("nulllllllllll");
                emp.c_image = existingEmp.c_image;
                Console.WriteLine("existing image: " + emp.c_image);
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
                emp.c_image = imageUrl;
            }
            var shift = Request.Form["c_shift"].ToList();
            emp.c_shift = string.Join(", ", shift);
            if (_employeeRepositories.UpdateEmployee(emp))
            {
                return Ok(emp);
            }
            return BadRequest(new { success = false, message = "Failed to update city" });
        }

        [HttpPut("ApiUpdate")]
        public IActionResult UpdateEmp(int id, [FromForm] Employee? emp = null)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("userid" , id);
            Console.WriteLine("userid sesssion " + _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
            var existingEmp = _employeeRepositories.FetchoneEmployee();
            if (existingEmp == null)
            {
                return NotFound();
            }

            if (emp.c_image == null || emp.c_image.Length == 0)
            {
                emp.c_image = existingEmp.c_image;
            }
            if (_employeeRepositories.UpdateEmployee(emp))
            {
                return Ok(emp);
            }
            return BadRequest(new { success = false, message = "Failed to update city" });
        }


        [HttpPost("Addemp")]
        public IActionResult Addemp([FromForm] Employee emp, IFormFile file)
        {
            Console.WriteLine("UserId  : " + emp.c_userid);
            Console.WriteLine("empid : " + emp.c_empid);
            Console.WriteLine("empname : " + emp.c_empname);
            Console.WriteLine("emp gender : " + emp.c_enpgender);
            Console.WriteLine("emp shift : " + emp.c_shift);
            Console.WriteLine("emp dob : " + emp.c_dob);
            Console.WriteLine("emp dept id : " + emp.c_dept_id);

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
            emp.c_image = imageUrl;

            var shift = Request.Form["c_shift"].ToList();
            emp.c_shift = string.Join(", ", shift);
            HttpContext.Session.SetInt32("userid", emp.c_userid.GetValueOrDefault());

            _employeeRepositories.addemp(emp);
            return Ok(emp);
            // return BadRequest(new { success = false, message = "Failed to add city" });
        }

        [HttpPost("ApiAdd")]
        public IActionResult AddEmployee([FromForm] Employee employee)
        {
            Console.WriteLine("call : " + employee.c_userid);
            HttpContext.Session.SetInt32("userid", employee.c_userid.GetValueOrDefault());
            if (_employeeRepositories.addemp(employee))
            {
                return Ok(employee);
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to add city" });
            }
        }

        [HttpPost("SaveImage")]
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


        [HttpDelete("delete{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeRepositories.DeletetEmployee(id);
            return Ok();

        }
    }
}