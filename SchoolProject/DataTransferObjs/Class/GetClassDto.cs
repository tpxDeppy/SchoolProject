﻿using SchoolProject.BL.Models;

namespace SchoolProject.API.DataTransferObjs.Class
{
    public class GetClassDto
    {
        public Guid Class_ID { get; set; }  //PK
        public string Class_name { get; set; }
        public string Class_description { get; set; }

    }
}
