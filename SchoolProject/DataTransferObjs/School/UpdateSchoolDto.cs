namespace SchoolProject.API.DataTransferObjs.School
{
    public class UpdateSchoolDto
    {
        public Guid School_ID { get; set; } //PK
        public string School_name { get; set; } = string.Empty;
    }
}
