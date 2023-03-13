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
            
            try
            {
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.User_ID == id);
                serviceResponse.Data = _mapper.Map<GetPersonDto>(dbPerson);

                if (dbPerson is null)
                {
                    throw new Exception($"Could not find a person with ID of '{id}'.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPersonDto>> GetPersonByLastName(string lastName)
        {
            var serviceResponse = new ServiceResponse<GetPersonDto>();

            try
            {
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.Last_name == lastName);
                serviceResponse.Data = _mapper.Map<GetPersonDto>(dbPerson);

                if (dbPerson is null)
                {
                    throw new Exception($"Could not find a person with the last name of '{lastName}'.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
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

            try
            {
                serviceResponse.Data = dbPupils.Select(_mapper.Map<GetPersonDto>)
                                           .Where(p => p.User_type == UserType.Pupil
                                                    && p.Year_group == yearGroup).ToList();

                if (serviceResponse.Data.Count == 0)
                {
                    throw new Exception($"Could not find pupils in this '{yearGroup}' year group.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

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

        public async Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson)
        {
            var serviceResponse = new ServiceResponse<GetPersonDto>();

            try
            {
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.User_ID == updatedPerson.User_ID);
                
                if (dbPerson is null)
                {
                    throw new Exception($"Person with ID '{updatedPerson.User_ID}' could not be found.");
                }

                dbPerson.First_name = updatedPerson.First_name;
                dbPerson.Last_name = updatedPerson.Last_name;
                dbPerson.User_type = updatedPerson.User_type;
                dbPerson.Date_of_birth = updatedPerson.Date_of_birth;
                dbPerson.Year_group = updatedPerson.Year_group;

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetPersonDto>(dbPerson);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> DeletePerson(Guid id)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();

            try
            {
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.User_ID == id);
                
                if (dbPerson is null)
                {
                    throw new Exception($"Person with ID '{id}' could not be found.");
                }

                _dataContext.Person.Remove(dbPerson);

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = 
                    await _dataContext.Person.Select(p => _mapper.Map<GetPersonDto>(p)).ToListAsync();
                
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

    }
}
