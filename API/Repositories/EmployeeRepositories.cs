using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; // Make sure this namespace is correct
using Npgsql;

namespace API.Repositories
{
    public class EmployeeRepositories : CommonRepositories, IEmployeeRepositories
    {
        public List<Employee> getAllEmployee()
        {
            var employlist = new List<Employee>();
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_enpgender, e.c_shift, e.c_dept_id, e.c_image , d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid", connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var empl = new Employee
                    {
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_enpgender = reader["c_enpgender"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_dept_id = Convert.ToInt32(reader["c_dept_id"]),
                        c_image = reader["c_image"].ToString(),
                        c_deptname = new API.Models.Dept
                        {
                            c_deptid = Convert.ToInt32(reader["c_dept_id"]),
                            c_deptname = reader.GetString(6)
                        }
                    };
                    employlist.Add(empl);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // Changed console to Console
            }
            finally
            {
                connection.Close();
            }
            return employlist;
        }
    }
}
