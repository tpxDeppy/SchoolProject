using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;
using System;
using System.Linq;

namespace SchoolProject.API.Services.PersonService
{
    public class PersonService : IPersonService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IValidator<Person> _validator;

        public PersonService(DataContext dataContext, IMapper mapper, IValidator<Person> validator) 
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetAllPeople()
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();

            try
            {
                var dbPeople = await _dataContext.Person.ToListAsync();
                serviceResponse.Data = dbPeople.Select(_mapper.Map<GetPersonDto>).ToList();

                if (dbPeople is null)
                {
                    throw new Exception($"Could not find any data...");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
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

            try
            {
                var dbPupils = await _dataContext.Person.ToListAsync();

                if (serviceResponse.Data is null || serviceResponse.Data.Count == 0)
                {
                    throw new Exception($"Could not find pupils in this '{yearGroup}' year group.");
                }

                serviceResponse.Data = dbPupils.Select(_mapper.Map<GetPersonDto>)
                                           .Where(p => p.User_type == UserType.Pupil
                                                    && p.Year_group == yearGroup).ToList();

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetPeopleFromSchool(Guid schoolID)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();

            try
            {
                var dbPeople = await _dataContext.Person.ToListAsync();

                if (serviceResponse.Data is null || serviceResponse.Data.Count == 0)
                {
                    throw new Exception($"School with ID of '{schoolID}' could not be found.");
                }

                serviceResponse.Data = dbPeople.Select(p => _mapper.Map<GetPersonDto>(p))
                                               .Where(p => p.School_ID == schoolID)
                                               .ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetPeopleInClass(Guid classID)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();

            try
            {
                var dbClass = await _dataContext.Class.ToListAsync();
                var dbPersonClass = await _dataContext.PersonClass.ToListAsync();
                var dbPerson = await _dataContext.Person.ToListAsync();

                if (dbPersonClass is null)
                {
                    throw new Exception($"Class with ID of '{classID}' could not be found.");
                }

                serviceResponse.Data = ( from c in dbClass
                                         join pc in dbPersonClass
                                         on c.Class_ID equals pc.Class_ID
                                         join p in dbPerson
                                         on pc.User_ID equals p.User_ID
                                         select _mapper.Map<GetPersonDto>(p)
                                        ).ToList();

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

            ValidationResult validationResult = await _validator.ValidateAsync(person);

            if (validationResult.IsValid)
            {
                _dataContext.Person.Add(person);
                await _dataContext.SaveChangesAsync();
            }
            
            serviceResponse.Data = 
                await _dataContext.Person.Select(p => _mapper.Map<GetPersonDto>(p)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson)
        {
            var serviceResponse = new ServiceResponse<GetPersonDto>();

            //attempted validation but I get Validation error when run it
            //ValidationResult validationResult = await _validator.ValidateAsync(_mapper.Map<Person>(updatedPerson));

            //if (!validationResult.IsValid)
            //{
            //    serviceResponse.Success = false;
            //    serviceResponse.Message = "Validation error.";
            //    return serviceResponse;
            //}

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
