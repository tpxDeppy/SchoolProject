using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;

namespace SchoolProject.Services.Interfaces
{
    public interface IPersonService
    {
        Task<ServiceResponse<List<GetPersonDto>>> GetAllPeople();
        Task<ServiceResponse<GetPersonDto>> GetPersonById(Guid id);
        Task<ServiceResponse<List<GetPersonDto>>> GetPersonBySearchParams(SearchPersonParamsDto searchParams);
        Task<ServiceResponse<GetPersonDto>> GetPersonByLastName(string lastName);
        Task<ServiceResponse<List<GetPersonDto>>> GetPeopleByUserType(UserType userType);
        Task<ServiceResponse<List<GetPersonDto>>> GetPupilsByYearGroup(int yearGroup);
        Task<ServiceResponse<List<GetPersonDto>>> GetPeopleFromSchool(Guid schoolID);
        Task<ServiceResponse<List<GetPersonDto>>> GetPeopleInClass(Guid classID);
        Task<ServiceResponse<List<GetPersonDto>>> GetPeopleInClassByName(string className);
        Task<ServiceResponse<List<GetPersonDto>>> AddPerson(AddPersonDto newPerson);
        Task<ServiceResponse<List<GetPersonDto>>> AddClassesToPerson(Guid userID, List<Guid> classIDs);
        Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson);
        Task<ServiceResponse<List<GetPersonDto>>> DeletePerson(Guid id);

    }
}
