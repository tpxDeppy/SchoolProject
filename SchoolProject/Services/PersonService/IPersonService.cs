using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.BL.Models;

namespace SchoolProject.API.Services.PersonService
{
    public interface IPersonService
    {
        Task<ServiceResponse<List<GetPersonDto>>> GetAllPeople();
        //Task<ServiceResponse<GetPersonDto>> GetPersonById(Guid id);
        ////list of persons to add person AddPerson(Person newPerson)
        //Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson);

    }
}
