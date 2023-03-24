
namespace SchoolProject.Models.Entities
{
    public class School
    {
        public Guid School_ID { get; set; } //PK
        public string School_name { get; set; } = string.Empty;

        public ICollection<Person> Persons { get; set; }
    }
}
