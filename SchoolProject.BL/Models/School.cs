using SchoolProject.BL.Models;

namespace SchoolProject.Models
{
    internal class School
    {
        public Guid School_ID { get; set; } //PK
        public string School_name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}
