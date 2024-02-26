using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Employee
    {
        public int c_empid { get; set; }
        public int c_userid { get; set; }
        public string c_empname { get; set; }
        public string c_gender { get; set; }
        public string c_shift { get; set; }
        public int c_dept_id { get; set; }
        public string c_image { get; set; }

        // Navigation properties for references
        public Dept Department { get; set; }
        public User SrsUser { get; set; }

    }
}