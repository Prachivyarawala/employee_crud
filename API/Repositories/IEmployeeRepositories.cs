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
        bool DeletetEmployee(int id);
        bool addemp(Employee employee);
        bool UpdateEmployee(Employee emp);
        List<Dept> GetAllDepartments();
    }
}