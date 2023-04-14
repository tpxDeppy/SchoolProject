
namespace SchoolProject.Models.Entities
{
    public class Class
    {
        public Guid ClassID { get; set; }  //PK
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<PersonClass> PersonClasses { get; set; }

    }
}
