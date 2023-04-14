﻿using Microsoft.AspNetCore.Mvc;
using SchoolProject.Services.Interfaces;
using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Models.Entities;

namespace SchoolProject.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet("AllClasses")]
        public async Task<ActionResult<ServiceResponse<List<GetClassDto>>>> GetAllClasses()
        {
            var response = await _classService.GetAllClasses();

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetClassDto>>> GetClassById(Guid id)
        {
            var response = await _classService.GetClassById(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("Class/{className}")]
        public async Task<ActionResult<ServiceResponse<GetClassDto>>> GetClassByName(string className)
        {
            var response = await _classService.GetClassByName(className);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("AddClass")]
        public async Task<ActionResult<List<GetClassDto>>> AddClass(AddClassDto newClass)
        {
            var response = await _classService.AddClass(newClass);
            return Created(nameof(GetClassById), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetClassDto>> UpdateClass(Guid id, UpdateClassDto updatedClass)
        {
            if (id != updatedClass.ClassID)
            {
                return BadRequest("Bad request. Please check that the IDs match.");
            }

            var response = await _classService.UpdateClass(updatedClass);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<GetClassDto>>> DeleteClass(Guid id)
        {
            var response = await _classService.DeleteClass(id);

            if (response.Data is null)
            {
                return NotFound(response.Message);
            }

            return Ok($"Class with ID of '{id}' was successfully deleted.");
        }
    }
}
