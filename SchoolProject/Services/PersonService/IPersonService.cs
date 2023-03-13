using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;

namespace SchoolProject.API.Services.PersonService
{
    public interface IPersonService
    {
        Task<ServiceResponse<List<GetPersonDto>>> GetAllPeople();
        Task<ServiceResponse<GetPersonDto>> GetPersonById(Guid id);
        Task<ServiceResponse<GetPersonDto>> GetPersonByLastName(string lastName);
        Task<ServiceResponse<List<GetPersonDto>>> GetPeopleByUserType(UserType userType);
        Task<ServiceResponse<List<GetPersonDto>>> GetPupilsByYearGroup(int yearGroup);
        Task<ServiceResponse<List<GetPersonDto>>> AddPerson(AddPersonDto newPerson);
        Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson);
        Task<ServiceResponse<List<GetPersonDto>>> DeletePerson(Guid id);

    }
}
