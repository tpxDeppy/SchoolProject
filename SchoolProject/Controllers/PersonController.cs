using Microsoft.AspNetCore.Mvc;
using SchoolProject.Services.Interfaces;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;
using Microsoft.AspNetCore.Cors;

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
        [EnableCors("AllowTrustedOrigins")]
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
        [EnableCors("AllowTrustedOrigins")]
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
        [EnableCors("AllowTrustedOrigins")]
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
        [EnableCors("AllowTrustedOrigins")]
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
        [EnableCors("AllowTrustedOrigins")]
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
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> GetPeopleFromSchool(Guid schoolID)
        {
            var response = await _personService.GetPeopleFromSchool(schoolID);

            if (response.Data is null || response.Data.Count == 0)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{classID}/peopleInClass")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> GetPeopleInClass(Guid classID)
        {
            var response = await _personService.GetPeopleInClass(classID);

            if (response.Data is null || response.Data.Count == 0)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{className}/peopleInClassByName")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> GetPeopleInClassByName(string className)
        {
            var response = await _personService.GetPeopleInClassByName(className);

            if (response.Data is null || response.Data.Count == 0)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("AddPerson")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<ServiceResponse<List<GetPersonDto>>>> AddPerson(AddPersonDto newPerson)
        {
            var response = await _personService.AddPerson(newPerson);
            return Created(nameof(GetSinglePerson), response);
        }

        [HttpPut("{id}")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> UpdatePerson(Guid id, UpdatePersonDto updatedPerson)
        {
            if (id != updatedPerson.UserID)
            {
                return BadRequest("Bad request. Please check that the IDs match.");
            }

            var response = await _personService.UpdatePerson(updatedPerson);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> DeletePerson(Guid id)
        {
            var response = await _personService.DeletePerson(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok($"Person with ID of '{id}' was successfully deleted.");
        }

    }
}
