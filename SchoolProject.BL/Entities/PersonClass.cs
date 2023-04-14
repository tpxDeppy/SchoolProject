
namespace SchoolProject.Models.Entities
{
    //represents the join table of Person and Class
    public class PersonClass
        {
            public Guid ClassID { get; set; }  //PK FK
            public Class Class { get; set; }
            public Guid UserID { get; set; }   //PK FK
            public Person Person { get; set; }

        }
}
