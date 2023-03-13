using SchoolProject.BL.Models.Enums;
using SchoolProject.BL.Models;

namespace SchoolProject.API.DataTransferObjs.Person
{
    public class AddPersonDto
    {
        public string First_name { get; set; } = string.Empty;
        public string Last_name { get; set; } = string.Empty;
        public UserType User_type { get; set; }
        public DateTime? Date_of_birth { get; set; } 
        public int? Year_group { get; set; } 

        public Guid School_ID { get; set; } 
        

    }
}
