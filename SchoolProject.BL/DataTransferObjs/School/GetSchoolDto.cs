
namespace SchoolProject.Models.DataTransferObjs.School
{
    public class GetSchoolDto
    {
        public Guid SchoolID { get; set; } //PK
        public string SchoolName { get; set; } = string.Empty;

        public ICollection<Entities.Person> Persons { get; set; }
    }
}
