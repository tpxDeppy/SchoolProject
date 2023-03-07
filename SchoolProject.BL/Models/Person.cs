using SchoolProject.BL.Models.Enums;

namespace SchoolProject.BL.Models
{
    public class Person
    {
        public Guid User_ID { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public UserType User_type { get; set; }
        public DateTime Date_of_birth { get; set; } //can be null
        public int Year_group { get; set; } //can be null

        public Guid School_ID { get; set; } //FK
        public School School { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<Class> Classes { get; set; }

    }
}
