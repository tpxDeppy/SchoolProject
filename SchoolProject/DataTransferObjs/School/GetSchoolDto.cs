namespace SchoolProject.API.DataTransferObjs.School
{
    public class GetSchoolDto
    {
        public Guid School_ID { get; set; } //PK
        public string School_name { get; set; } = string.Empty;

        public ICollection<BL.Models.Person> Persons { get; set; }
    }
}
