using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models; 
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
               
                connection.Open();

                // Create and execute the SQL command
                var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_shift, e.c_dept_id, d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid", connection);
                var reader = cmd.ExecuteReader();

                // Read data from the result set
                while (reader.Read())
                {
                    var empl = new AdminEmployee
                    {
                        
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_dept_id = Convert.ToInt32(reader["c_dept_id"]),
                   
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
                Console.WriteLine(e);                 
                throw; 
            }
            finally
            {
                connection.Close(); 
            }
            return employlist; 
        }

        public void UpdateEmployee(AdminEmployee employee)
{
    try
    {
        
        connection.Open();

        
        var cmd = new NpgsqlCommand("UPDATE public.t_employee SET c_empname = @empname, c_shift = @shift, c_dept_id = @deptid WHERE c_empid = @empid", connection);

        
        cmd.Parameters.AddWithValue("@empname", employee.c_empname);
        cmd.Parameters.AddWithValue("@shift", employee.c_shift);
        cmd.Parameters.AddWithValue("@deptid", employee.c_dept_id);
        cmd.Parameters.AddWithValue("@empid", employee.c_empid);

        
        cmd.ExecuteNonQuery();
    }
    catch (Exception e)
    {
        Console.WriteLine(e); 
        throw; 
        }
    finally
    {
        connection.Close(); 
    }
}
public List<Dept> GetAllDepartments()
{
    var deptList = new List<Dept>();
    try
    {
        
        connection.Open();

        
        var cmd = new NpgsqlCommand("SELECT c_deptid, c_deptname FROM public.t_dept", connection);
        var reader = cmd.ExecuteReader();

        
        while (reader.Read())
        {
            var dept = new Dept
            {
                
                c_deptid = Convert.ToInt32(reader["c_deptid"]),
                c_deptname = reader["c_deptname"].ToString()
            };
            deptList.Add(dept); 
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e); 
        throw; 
    }
    finally
    {
        connection.Close(); 
    }
    return deptList;
}
public AdminEmployee FetchByEmpid(int empId)
{
    AdminEmployee employee = null;
    try
    {
        connection.Open();
        var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_shift, e.c_dept_id, d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid WHERE e.c_empid = @empid", connection);
        cmd.Parameters.AddWithValue("@empid", empId);
        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            employee = new AdminEmployee
            {
                c_empid = reader.GetInt32(0),
                c_empname = reader.GetString(1),
                c_shift = reader.GetString(2),
                c_dept_id = reader.GetInt32(3),
                c_deptname = new Dept
                {
                    c_deptid = reader.GetInt32(3),
                    c_deptname = reader.GetString(4)
                }
            };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    finally
    {
        connection.Close();
    }
    return employee;
}


    }
}
