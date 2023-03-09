using SchoolProject.BL.Models.Enums;
using SchoolProject.BL.Models;

namespace SchoolProject.API.DataTransferObjs.Person
{
    public class UpdatePersonDto
    {
        public Guid User_ID { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public UserType User_type { get; set; }
        public DateTime Date_of_birth { get; set; }
        public int Year_group { get; set; }

        public Guid School_ID { get; set; }
        public School School { get; set; }

        public ICollection<PersonClass> PersonClasses { get; set; }
    }
}
