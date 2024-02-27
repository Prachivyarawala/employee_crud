using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; 

namespace API.Repositories
{
    public interface IEmployeeRepositories
    {
        List<Employee> getAllEmployee();
        Employee FetchoneEmployee();

        List<Dept> GetAllDepartments();
        
    }
}