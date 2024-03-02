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
    [Route("api/[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly IAdminRepositories _adminRepo;

        public AdminApiController(IAdminRepositories adminRepo)
        {
            _adminRepo = adminRepo;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _adminRepo.getAllEmployee();
            if (employees == null || employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpPost]
        public IActionResult EditEmployee([FromForm] AdminEmployee employee)
        {
            if (!_adminRepo.UpdateEmployee(employee))
            {
                return BadRequest("Failed to update employee.");
            }

            return Ok(employee);
        }

        [HttpGet("departments")]
        public IActionResult GetAllDepartments()
        {
            var departments = _adminRepo.GetAllDepartments();
            if (departments == null || departments.Count == 0)
            {
                return NoContent();
            }

            return Ok(departments);
        }
        [HttpGet("{id}")]
        public ActionResult<AdminEmployee> GetEmployeeById(int id)
        {
            var employee = _adminRepo.FetchByEmpid(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }
    }
}