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
    public class EmpApiController : ControllerBase
    {
        private readonly IEmployeeRepositories _emprepo;
        public EmpApiController(IEmployeeRepositories emprepo)
        {
            _emprepo = emprepo;
        }
        
        [HttpGet]
        public List<Employee> Get()
        {
            return _emprepo.getAllEmployee();
            // int.Parse(HttpContext.User.Claims.First(i =>i.Type == "UserId").Value)
        }
    }
}