using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.API.Services.PersonService;
using SchoolProject.BL.Models;
using SchoolProject.BL.Models.Enums;
using SchoolProject.DAL;
using System.Drawing.Text;

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
            
            return Ok(await _personService.GetAllPeople());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetSinglePerson(Guid id)
        {
            return Ok(await _personService.GetPersonById(id));
        }

        [HttpGet("User/{lastName}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetPersonByLastName(string lastName)
        {
            return Ok(await _personService.GetPersonByLastName(lastName));
        }

        [HttpGet("UserType/{userType}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetPersonByUserType(UserType userType)
        {
            return Ok(await _personService.GetPersonByUserType(userType));
        }

        [HttpGet("Pupil/{yearGroup}")]
        public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetPupilsByYearGroup(int yearGroup)
        {
            return Ok(await _personService.GetPupilsByYearGroup(yearGroup));
        }

        //[HttpPost]
        //public async Task<ActionResult<ServiceResponse<GetPersonDto>>> AddPerson(Person newPerson) 
        //{
        //    persons.Add(newPerson);
        //    return Ok(newPerson);
        //}

        //[HttpPut]
        //public async Task<ActionResult<ServiceResponse<GetPersonDto>>> UpdatePerson(UpdatePersonDto updatedPerson)
        //{
        //    return Ok(await _personService.UpdatePerson(updatedPerson));
        //}


    }
}
