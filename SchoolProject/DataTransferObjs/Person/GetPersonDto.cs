using SchoolProject.BL.Models.Enums;

namespace SchoolProject.API.DataTransferObjs.Person
{
    //Initially it's the same as the Person model
    //but later on it will contain only the things that we want the user to see
    public class GetPersonDto
    {
        public Guid User_ID { get; set; }
        public string First_name { get; set; } = string.Empty;
        public string Last_name { get; set; } = string.Empty;
        public UserType User_type { get; set; }
        public DateTime? Date_of_birth { get; set; } 
        public int? Year_group { get; set; } 

        public Guid School_ID { get; set; }
    }
}
