using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; 
namespace API.Repositories
{
    public interface IAdminRepositories
    {
        List<AdminEmployee> getAllEmployee();
        bool  UpdateEmployee(AdminEmployee employee);
        List<Dept> GetAllDepartments();
        AdminEmployee FetchByEmpid(int empId);
    }
}