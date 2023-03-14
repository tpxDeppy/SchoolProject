using Azure;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.API.Services.PersonService;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        //Injecting IPersonService
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<GetPersonDto>>> GetAll()
        {
            var response = await _personService.GetAllPeople();
            
            if (response.Data is null) 
            { 
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetSinglePerson(Guid id)
        {
            var response = await _personService.GetPersonById(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("User/{lastName}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetPersonByLastName(string lastName)
        {
            var response = await _personService.GetPersonByLastName(lastName);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("UserType/{userType}")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> GetPersonByUserType(UserType userType)
        {
            var response = await _personService.GetPeopleByUserType(userType);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("Pupil/{yearGroup}")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> GetPupilsByYearGroup(int yearGroup)
        {
            var response = await _personService.GetPupilsByYearGroup(yearGroup);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{schoolID}/people")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> GetPeopleFromSchool(Guid schoolID)
        {
            var response = await _personService.GetPeopleFromSchool(schoolID);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("AddPerson")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> AddPerson(AddPersonDto newPerson)
        {
            return Ok(await _personService.AddPerson(newPerson));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> UpdatePerson(UpdatePersonDto updatedPerson)
        {
            var response = await _personService.UpdatePerson(updatedPerson);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> DeletePerson(Guid id)
        {
            var response = await _personService.DeletePerson(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

    }
}
