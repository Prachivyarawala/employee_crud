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
        public IActionResult getemployee()
        {
            var emp = _employeeRepositories.FetchoneEmployee();
            return Ok(emp);
        }
        

        [HttpPost("Addemp")]
        public IActionResult Addemp([FromForm] Employee emp ,IFormFile file )
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

            var imageUrl = Path.Combine("/images", file.FileName);
            emp.c_image = imageUrl;
            HttpContext.Session.SetInt32("userid", emp.c_userid);

           _employeeRepositories.addemp(emp);
            return Ok();
            // return BadRequest(new { success = false, message = "Failed to add city" });
        }
    }
}