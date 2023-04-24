
namespace SchoolProject.Models.DataTransferObjs.Class
{
    public class GetClassDto
    {
        public Guid ClassID { get; set; }  //PK
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }

    }
}
