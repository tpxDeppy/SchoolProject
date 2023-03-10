using AutoMapper;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;

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
            serviseResponse.Data = dbPeople.Select(_mapper.Map<GetPersonDto>).ToList();
            return serviseResponse;
        }
        public async Task<ServiceResponse<GetPersonDto>> GetPersonById(Guid id)
        {
            var serviceResponse = new ServiceResponse<GetPersonDto>();
            var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.User_ID == id);
            serviceResponse.Data = _mapper.Map<GetPersonDto>(dbPerson);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPersonDto>> GetPersonByLastName(string lastName)
        {
            var serviceResponse = new ServiceResponse<GetPersonDto>();
            var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.Last_name == lastName);
            serviceResponse.Data = _mapper.Map<GetPersonDto>(dbPerson);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetPeopleByUserType(UserType userType)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();
            var dbPeople = await _dataContext.Person.ToListAsync();
            serviceResponse.Data = dbPeople.Select(_mapper.Map<GetPersonDto>)
                                           .Where(p => p.User_type == userType).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetPupilsByYearGroup(int yearGroup)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();
            var dbPupils = await _dataContext.Person.ToListAsync();
            serviceResponse.Data = dbPupils.Select(_mapper.Map<GetPersonDto>)
                                           .Where(p => p.User_type == UserType.Pupil).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> AddPerson(AddPersonDto newPerson)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();
            var person = _mapper.Map<Person>(newPerson);

            _dataContext.Person.Add(person);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Data = 
                await _dataContext.Person.Select(p => _mapper.Map<GetPersonDto>(p)).ToListAsync();
            return serviceResponse;
        }


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
