using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Models.DataTransferObjs.Person
{
    //Initially it's the same as the Person model
    //but later on it will contain only the things that we want the user to see
    public class GetPersonDto
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public DateTime? DateOfBirth { get; set; } 
        public int? YearGroup { get; set; } 

        public Guid SchoolID { get; set; }

        public ICollection<PersonClass> PersonClasses { get; set; }
    }
}
