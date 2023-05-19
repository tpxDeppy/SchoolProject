using Microsoft.AspNetCore.Mvc;
using SchoolProject.Services.Interfaces;
using SchoolProject.Models.DataTransferObjs.School;
using Microsoft.AspNetCore.Cors;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet("All")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<List<GetSchoolDto>>> GetSchools()
        {
            var response = await _schoolService.GetSchools();

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<GetSchoolDto>> GetSchoolById(Guid id)
        {
            var response = await _schoolService.GetSchoolById(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("School/{schoolName}")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<List<GetSchoolDto>>> GetSchoolByName(string schoolName)
        {
            var response = await _schoolService.GetSchoolByName(schoolName);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("AddSchool")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<List<GetSchoolDto>>> AddSchool(AddSchoolDto newSchool)
        {
            var response = await _schoolService.AddSchool(newSchool);
            return Created(nameof(GetSchoolById), response);
        }

        [HttpPut("{id}")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<GetSchoolDto>> UpdateSchool(Guid id, UpdateSchoolDto updatedSchool)
        {
            if (id != updatedSchool.SchoolID)
            {
                return BadRequest("Bad request. Please check that the IDs match.");
            }

            var response = await _schoolService.UpdateSchool(updatedSchool);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [EnableCors("AllowTrustedOrigins")]
        public async Task<ActionResult<GetSchoolDto>> DeleteSchool(Guid id)
        {
            var response = await _schoolService.DeleteSchool(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok($"School with ID of '{id}' was successfully deleted.");
        }
    }
}
