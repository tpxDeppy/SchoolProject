
namespace SchoolProject.Models.Entities
{
    //represents the join table of Person and Class
    public class PersonClass
        {
            public Guid Class_ID { get; set; }  //PK FK
            public Class Class { get; set; }
            public Guid User_ID { get; set; }   //PK FK
            public Person Person { get; set; }

        }
}
