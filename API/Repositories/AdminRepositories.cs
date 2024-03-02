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
                using (var connection = new NpgsqlConnection("server=cipg01;Port=5432;Database=Group_H;User Id=postgres;Password=123456;"))
                {
                    connection.Open();
                    var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_shift, e.c_dept_id, d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid", connection);
                    using (var reader = cmd.ExecuteReader())
                    {
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
                }
            }
            catch (Exception e)
            {
                // Log the exception
                Console.WriteLine(e);
                throw;
            }
            return employlist;
        }

        public bool UpdateEmployee(AdminEmployee employee)
        {
            try
            {
                using (var connection = new NpgsqlConnection("server=cipg01;Port=5432;Database=Group_H;User Id=postgres;Password=123456;"))
                {
                    connection.Open();
                    var cmd = new NpgsqlCommand("UPDATE public.t_employee SET c_empname = @empname, c_shift = @shift, c_dept_id = @deptid WHERE c_empid = @empid", connection);
                    cmd.Parameters.AddWithValue("@empid", employee.c_empid);
                    cmd.Parameters.AddWithValue("@empname", employee.c_empname);
                    cmd.Parameters.AddWithValue("@shift", employee.c_shift);
                    cmd.Parameters.AddWithValue("@deptid", employee.c_dept_id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                return false;
            }
        }
        public List<Dept> GetAllDepartments()
        {
            var deptList = new List<Dept>();
            try
            {
                using (var connection = new NpgsqlConnection("server=cipg01;Port=5432;Database=Group_H;User Id=postgres;Password=123456;"))
                {
                    connection.Open();
                    var cmd = new NpgsqlCommand("SELECT c_deptid, c_deptname FROM public.t_dept", connection);
                    using (var reader = cmd.ExecuteReader())
                    {
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
                }
            }
            catch (Exception e)
            {
                // Log the exception
                Console.WriteLine(e);
                throw;
            }
            return deptList;
        }
        public AdminEmployee FetchByEmpid(int empId)
        {
            AdminEmployee employee = null;
            try
            {
                using (var connection = new NpgsqlConnection("server=cipg01;Port=5432;Database=Group_H;User Id=postgres;Password=123456;"))
                {
                    connection.Open();
                    var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_empname, e.c_shift, e.c_dept_id, d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid WHERE e.c_empid = @empid", connection);
                    cmd.Parameters.AddWithValue("@empid", empId);
                    using (var reader = cmd.ExecuteReader())
                    {
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
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
            }
            return employee;
        }


    }
}
