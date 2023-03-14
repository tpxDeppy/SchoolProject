﻿using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.DataTransferObjs.School;
using SchoolProject.API.Services.SchoolService;

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
        public async Task<ActionResult<List<GetSchoolDto>>> AddSchool(AddSchoolDto newSchool)
        {
            return Ok(await _schoolService.AddSchool(newSchool));
        }

        [HttpPut]
        public async Task<ActionResult<GetSchoolDto>> UpdateSchool(UpdateSchoolDto updatedSchool)
        {
            var response = await _schoolService.UpdateSchool(updatedSchool);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GetSchoolDto>> DeleteSchool(Guid id)
        {
            var response = await _schoolService.DeleteSchool(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }
    }
}