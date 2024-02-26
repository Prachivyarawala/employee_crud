using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Employee
    {
        public int c_empid { get; set; }

        [ForeignKey("c_userid")]
        public int c_userid { get; set; }
        public virtual User SrsUser { get; set; }

        public string c_empname { get; set; }
        public string c_gender { get; set; }
        public string c_shift { get; set; }
     
        public string c_image { get; set; }

        // Navigation properties for references>

         [ForeignKey("c_dept_id")]
         public int c_dept_id { get; set; }
        public virtual Dept Department { get; set; }
        

    }
}