using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.BL.Models
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
