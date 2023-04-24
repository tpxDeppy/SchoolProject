
namespace SchoolProject.Models.Entities
{
    public class School
    {
        public Guid SchoolID { get; set; } //PK
        public string SchoolName { get; set; } = string.Empty;

        public ICollection<Person> Persons { get; set; }
    }
}
