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
        public EmpApiController(IEmployeeRepositories employeeRepositories)
        {
            _employeeRepositories = employeeRepositories;
        }



        [HttpGet("getemployee")]
        public IActionResult getemployee(int id)
        {
            HttpContext.Session.SetInt32("userid", id);
            var emp = _employeeRepositories.FetchoneEmployee();
            return Ok(emp);
        }

        [HttpPut("UpdateEmployee")]
        public IActionResult UpdateEmployee(int id,[FromForm] Employee? emp = null, IFormFile? file = null)
        {
            var existingEmp = _employeeRepositories.FetchoneEmployee();
            if (existingEmp == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                Console.WriteLine("nulllllllllll");
                emp.c_image = existingEmp.c_image;
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
                return Ok();
            }

            return BadRequest(new { success = false, message = "Failed to update city" });
        }


        [HttpPost("Addemp")]
        public IActionResult Addemp([FromForm] Employee emp, IFormFile file)
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
            
            var shift = Request.Form["c_shift"].ToList();
            emp.c_shift = string.Join(", ", shift);
            HttpContext.Session.SetInt32("userid", emp.c_userid.GetValueOrDefault());

            _employeeRepositories.addemp(emp);
            return Ok();
            // return BadRequest(new { success = false, message = "Failed to add city" });
        }


        [HttpDelete("delete{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeRepositories.DeletetEmployee(id);
            return Ok();

        }
    }
}