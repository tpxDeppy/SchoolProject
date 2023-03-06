using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SchoolProject.BL.Models
{
    internal class Class
    {
        public Guid Class_ID { get; set; }  //PK
        public string Class_name { get; set; }
        public string Class_description { get; set; }

        //implementing the many to many relationship between Person and Class
        public ICollection<ClassConnection> Persons { get; set; }
    
    }
}
