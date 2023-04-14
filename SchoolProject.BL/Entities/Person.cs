using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Models.Entities
{
    public class Person
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public DateTime? DateOfBirth { get; set; } //can be null
        public int? YearGroup { get; set; } //can be null

        public Guid SchoolID { get; set; } //FK
        public School School { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<PersonClass> PersonClasses { get; set; }

    }
}
