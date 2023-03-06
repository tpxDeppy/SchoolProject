using SchoolProject.BL.Models.Enums;
using SchoolProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.BL.Models
{
    internal class Person
    {
        public Guid User_ID { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public UserType User_type { get; set; }
        public DateOnly Date_of_birth { get; set; } //can be null
        public int Year_group { get; set; } //can be null

        public Guid School_ID { get; set; } //FK
        public School School { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<ClassConnection> Classes { get; set; }

    }
}
