
namespace SchoolProject.Models.DataTransferObjs.Class
{
    public class UpdateClassDto
    {
        public Guid ClassID { get; set; }  //PK
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }

    }
}
