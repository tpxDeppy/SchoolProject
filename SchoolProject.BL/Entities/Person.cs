﻿using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Models.Entities
{
    public class Person
    {
        public Guid User_ID { get; set; }
        public string First_name { get; set; } = string.Empty;
        public string Last_name { get; set; } = string.Empty;
        public UserType User_type { get; set; }
        public DateTime? Date_of_birth { get; set; } //can be null
        public int? Year_group { get; set; } //can be null

        public Guid School_ID { get; set; } //FK
        public School School { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<PersonClass> PersonClasses { get; set; }

    }
}