
namespace SchoolProject.Models.DataTransferObjs.School
{
    public class UpdateSchoolDto
    {
        public Guid SchoolID { get; set; } //PK
        public string SchoolName { get; set; } = string.Empty;
    }
}
