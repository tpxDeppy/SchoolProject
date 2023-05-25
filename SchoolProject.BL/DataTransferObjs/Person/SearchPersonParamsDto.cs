
namespace SchoolProject.Models.DataTransferObjs.Person
{
    public class SearchPersonParamsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public int? YearGroup { get; set; }
    }
}
