using AutoMapper;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.BL.Models;

namespace SchoolProject.API.Services.PersonService
{
    public class PersonService : IPersonService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public PersonService(DataContext dataContext, IMapper mapper) 
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetAllPeople()
        {
            var serviseResponse = new ServiceResponse<List<GetPersonDto>>();
            var dbPeople = await _dataContext.Person.ToListAsync();
            serviseResponse.Data = dbPeople.Select(p => _mapper.Map<GetPersonDto>(p)).ToList();
            return serviseResponse;
        }
        //public async Task<ServiceResponse<GetPersonDto>> GetPersonById(Guid id)
        //{
        //    var serviceResponse = new ServiceResponse<GetPersonDto>();
        //    var person = persons.FirstOrDefault(p => p.User_ID == id);
        //    serviceResponse.Data = _mapper.Map<GetPersonDto>(person);
        //    return serviceResponse;
        //}

        //public async Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson)
        //{
        //    var serviceResponse = new ServiceResponse<GetPersonDto>();
        //    var person = persons.FirstOrDefault(p => p.User_ID == updatedPerson.User_ID);

        //    person.First_Name = updatedPerson.First_name;
        //    //etc

        //    serviceResponse.Data = _mapper.Map<GetPersonDto>(updatedPerson);
        //    return serviceResponse;
        //}
    }
}
