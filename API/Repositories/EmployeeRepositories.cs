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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeRepositories(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
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


        public Employee FetchoneEmployee()
        {
            var employee = new Employee();
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT e.c_empid, e.c_userid, e.c_empname, e.c_enpgender, e.c_shift, e.c_dept_id, e.c_image , d.c_deptname FROM public.t_employee e INNER JOIN public.t_dept d ON e.c_dept_id = d.c_deptid WHERE e.c_userid=@c_userid", connection);
                cmd.Parameters.AddWithValue("@c_userid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee.c_empid = Convert.ToInt32(reader["c_empid"]); // Fixed assignment statement
                    employee.c_empname = reader["c_empname"].ToString();
                    employee.c_enpgender = reader["c_enpgender"].ToString();
                    employee.c_shift = reader["c_shift"].ToString();
                    employee.c_dept_id = Convert.ToInt32(reader["c_dept_id"]);
                    employee.c_image = reader["c_image"].ToString();
                    employee.c_deptname = new API.Models.Dept
                    {
                        c_deptid = Convert.ToInt32(reader["c_dept_id"]),
                        c_deptname = reader.GetString(7)
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


        public void DeletetEmployee(int id)
        {
            try
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM public.t_employee WHERE c_empid=@c_empid", connection);

                cmd.Parameters.AddWithValue("@c_empid", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting task: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void addemp(Employee employee)
        {
            try
            {
                connection.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO t_employee(c_userid, c_empname, c_enpgender, c_shift, c_dept_id, c_image, c_dob) VALUES (@c_userid, @c_empname, @c_enpgender, @c_shift, @c_dept_id, @c_image, @c_dob)", connection);

                cmd.Parameters.AddWithValue("@c_userid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
                cmd.Parameters.AddWithValue("@c_empname", employee.c_empname);
                cmd.Parameters.AddWithValue("@c_enpgender", employee.c_enpgender);
                cmd.Parameters.AddWithValue("@c_shift", employee.c_shift);
                cmd.Parameters.AddWithValue("@c_dept_id", employee.c_dept_id);
                cmd.Parameters.AddWithValue("@c_image", employee.c_image);
                cmd.Parameters.AddWithValue("@c_dob", employee.c_dob);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding task: " + ex.Message);
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




        public bool UpdateEmployee(Employee emp)
        {
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("UPDATE public.t_employee SET c_empname = @empname, c_enpgender = @enpgender, c_shift = @shift, c_dept_id = @deptid, c_image = @image, c_dob = @dob WHERE c_empid = @empid", connection);
                cmd.Parameters.AddWithValue("@empid", emp.c_empid);
                cmd.Parameters.AddWithValue("@empname", emp.c_empname);
                cmd.Parameters.AddWithValue("@enpgender", emp.c_enpgender);
                cmd.Parameters.AddWithValue("@shift", emp.c_shift);
                cmd.Parameters.AddWithValue("@deptid", emp.c_dept_id);
                cmd.Parameters.AddWithValue("@image", emp.c_image);
                cmd.Parameters.AddWithValue("@dob", emp.c_dob);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }


    }
}
