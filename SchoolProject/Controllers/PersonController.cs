using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.API.Services.PersonService;
using SchoolProject.BL.Models;
using SchoolProject.DAL;
using System.Drawing.Text;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IPersonService _personService;
        public PersonController(DataContext dataContext, IPersonService personService) 
        { 
            _dataContext = dataContext;
            _personService = personService;
            
        }

        private static Person pupil = new Person();

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<GetPersonDto>>> GetAll()
        {
            
            return Ok(await _personService.GetAllPeople());
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ServiceResponse<GetPersonDto>>> GetSinglePerson(Guid id)
        //{
        //    return Ok(await _personService.GetPersonById(id));
        //}

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
