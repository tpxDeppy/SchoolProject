
namespace SchoolProject.BL.Models
{
    public class School
    {
        public Guid School_ID { get; set; } //PK
        public string School_name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}
