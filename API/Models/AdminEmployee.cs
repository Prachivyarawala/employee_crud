using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models
{
    public class AdminEmployee
    {
        public int c_empid { get; set; }
        public string c_empname { get; set; }
        public string c_shift { get; set; }

        [ForeignKey("c_dept_id")]
        public int c_dept_id { get; set; }
        public virtual Dept c_deptname { get; set; }
    }
}