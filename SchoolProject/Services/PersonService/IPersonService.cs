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
        Task<ServiceResponse<GetPersonDto>> GetPersonByUserType(UserType userType);
        Task<ServiceResponse<List<GetPersonDto>>> GetPupilsByYearGroup(int yearGroup);
        ////list of persons to add person AddPerson(Person newPerson)
        //Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson);

    }
}
