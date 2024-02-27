using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; // Make sure this namespace is correct
using Npgsql;

namespace API.Repositories
{
    public class AdminRepositories : CommonRepositories, IAdminRepositories
    {
        public List<AdminEmployee> getAllEmployee()
        {
            var employlist = new List<AdminEmployee>();
            try
            {
                // Open the database connection
                connection.Open();

                // Create and execute the SQL command
                var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_shift, e.c_dept_id, d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid", connection);
                var reader = cmd.ExecuteReader();

                // Read data from the result set
                while (reader.Read())
                {
                    var empl = new AdminEmployee
                    {
                        // Read employee attributes from the data reader
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_dept_id = Convert.ToInt32(reader["c_dept_id"]),
                        // Create department object and set its properties
                        c_deptname = new Dept
                        {
                            c_deptid = Convert.ToInt32(reader["c_dept_id"]),
                            c_deptname = reader["c_deptname"].ToString()
                        }
                    };
                    employlist.Add(empl); // Add employee object to the list
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // Log or handle the exception appropriately
                throw; // Rethrow the exception to be handled higher up in the call stack
            }
            finally
            {
                connection.Close(); // Close the database connection in the finally block
            }
            return employlist; // Return the list of employees
        }
    }
}
