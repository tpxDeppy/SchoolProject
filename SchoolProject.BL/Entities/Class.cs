
namespace SchoolProject.Models.Entities
{
    public class Class
    {
        public Guid Class_ID { get; set; }  //PK
        public string Class_name { get; set; }
        public string Class_description { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<PersonClass> PersonClasses { get; set; }

    }
}
