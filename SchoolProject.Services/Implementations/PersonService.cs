using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;
using SchoolProject.Data;
using SchoolProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SchoolProject.Services.Implementations
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

                if (serviceResponse.Data is null || serviceResponse.Data.Count == 0)
                {
                    throw new Exception("Could not find any data...");
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
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.UserID == id);
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
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.LastName == lastName);
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
                                           .Where(p => p.UserType == userType)
                                           .ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetPupilsByYearGroup(int yearGroup)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();

            try
            {
                var dbPupils = await _dataContext.Person.ToListAsync();

                if (dbPupils is null)
                {
                    throw new Exception($"Could not find pupils in this '{yearGroup}' year group.");
                }

                serviceResponse.Data = dbPupils.Select(_mapper.Map<GetPersonDto>)
                                               .Where(p => p.UserType == UserType.Pupil && p.YearGroup == yearGroup)
                                               .ToList();

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

                serviceResponse.Data = dbPeople.Select(p => _mapper.Map<GetPersonDto>(p))
                                               .Where(p => p.SchoolID == schoolID)
                                               .ToList();

                if (serviceResponse.Data is null || serviceResponse.Data.Count == 0)
                {
                    throw new Exception($"Could not find people in the school with ID of '{schoolID}'.");
                }
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
                var dbPeopleInClass = await _dataContext.Person
                                            .Include(pc => pc.PersonClasses)
                                            .Where(p => p.PersonClasses.Any(pc => pc.ClassID == classID))
                                            .ToListAsync();                                            

                serviceResponse.Data = dbPeopleInClass.Select(_mapper.Map<GetPersonDto>).ToList();

                if (serviceResponse.Data is null || serviceResponse.Data.Count == 0)
                {
                    throw new Exception($"Could not find people in the class with ID of '{classID}'.");
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPersonDto>>> GetPeopleInClassByName(string className)
        {
            var serviceResponse = new ServiceResponse<List<GetPersonDto>>();

            try
            {
                var dbPeopleInClass = await _dataContext.Person
                                           .Include(pc => pc.PersonClasses)
                                           .Where(p => p.PersonClasses.Any(pc => pc.Class.ClassName == className))
                                           .ToListAsync();

                serviceResponse.Data = dbPeopleInClass.Select(_mapper.Map<GetPersonDto>).ToList();

                if (serviceResponse.Data is null || serviceResponse.Data.Count == 0)
                {
                    throw new Exception($"Could not find people in the class with the name of '{className}'.");
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

            ValidationResult validationResult = await _validator.ValidateAsync(person);

            if (!validationResult.IsValid)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Validation error. Person was not saved.";
                return serviceResponse;
            }

            _dataContext.Person.Add(person);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = $"Successfully created new person with the first name of '{newPerson.FirstName}'.";
            serviceResponse.Data =
                await _dataContext.Person.Select(p => _mapper.Map<GetPersonDto>(p)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPersonDto>> UpdatePerson(UpdatePersonDto updatedPerson)
        {
            var serviceResponse = new ServiceResponse<GetPersonDto>();

            ValidationResult validationResult = await _validator.ValidateAsync(_mapper.Map<Person>(updatedPerson));

            if (!validationResult.IsValid)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Validation error. Person was not updated.";
                return serviceResponse;
            }

            try
            {
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.UserID == updatedPerson.UserID);

                if (dbPerson is null)
                {
                    throw new Exception($"Person with ID '{updatedPerson.UserID}' could not be found.");
                }

                dbPerson = _mapper.Map(updatedPerson, dbPerson);

                await _dataContext.SaveChangesAsync();

                serviceResponse.Message = "Successfully updated.";
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
                var dbPerson = await _dataContext.Person.FirstOrDefaultAsync(p => p.UserID == id);

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
