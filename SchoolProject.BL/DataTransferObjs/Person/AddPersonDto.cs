using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Models.DataTransferObjs.Person
{
    public class AddPersonDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public DateTime? DateOfBirth { get; set; } 
        public int? YearGroup { get; set; } 

        public Guid SchoolID { get; set; } 
        

    }
}
