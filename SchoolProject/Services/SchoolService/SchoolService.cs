﻿using AutoMapper;
using SchoolProject.API.DataTransferObjs.School;
using SchoolProject.BL.Models;

namespace SchoolProject.API.Services.SchoolService
{
    public class SchoolService : ISchoolService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public SchoolService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetSchoolDto>>> GetSchools()
        {
            var serviceResponse = new ServiceResponse<List<GetSchoolDto>>();

            try
            {
                var dbSchools = await _dataContext.School.ToListAsync();
                serviceResponse.Data = dbSchools.Select(_mapper.Map<GetSchoolDto>).ToList();

                if (dbSchools is null)
                {
                    throw new Exception($"Could not find any school data...");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSchoolDto>> GetSchoolById(Guid id)
        {
            var serviceResponse = new ServiceResponse<GetSchoolDto>();

            try
            {
                var dbSchool = await _dataContext.School.FirstOrDefaultAsync(s => s.School_ID == id);
                serviceResponse.Data = _mapper.Map<GetSchoolDto>(dbSchool);

                if (dbSchool is null)
                {
                    throw new Exception($"Could not find a school with ID of '{id}'.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSchoolDto>> GetSchoolByName(string schoolName)
        {
            var serviceResponse = new ServiceResponse<GetSchoolDto>();

            try
            {
                var dbSchool = await _dataContext.School.FirstOrDefaultAsync(s => s.School_name == schoolName);
                serviceResponse.Data = _mapper.Map<GetSchoolDto>(dbSchool);

                if (dbSchool is null)
                {
                    throw new Exception($"School with the name of '{schoolName}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
                
        public async Task<ServiceResponse<List<GetSchoolDto>>> AddSchool(AddSchoolDto newSchool)
        {
            var serviceResponse = new ServiceResponse<List<GetSchoolDto>>();
            var school = _mapper.Map<School>(newSchool);

            _dataContext.School.Add(school);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Data = 
                await _dataContext.School.Select(s => _mapper.Map<GetSchoolDto>(s)).ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetSchoolDto>> UpdateSchool(UpdateSchoolDto updatedSchool)
        {
            var serviceResponse = new ServiceResponse<GetSchoolDto>();

            try
            {
                var dbSchool = await _dataContext.School.FirstOrDefaultAsync(s => s.School_ID == updatedSchool.School_ID);

                if (dbSchool is null)
                {
                    throw new Exception($"School with ID of '{updatedSchool.School_ID}' could not be found.");
                }

                dbSchool.School_name = updatedSchool.School_name;

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetSchoolDto>(dbSchool);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetSchoolDto>>> DeleteSchool(Guid id)
        {
            var serviceResponse = new ServiceResponse<List<GetSchoolDto>>();

            try
            {
                var dbSchool = await _dataContext.School.FirstOrDefaultAsync(s => s.School_ID == id);

                if (dbSchool is null)
                {
                    throw new Exception($"School with ID of '{id}' could not be found.");
                }

                _dataContext.Remove(dbSchool);

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = 
                    await _dataContext.School.Select(s => _mapper.Map<GetSchoolDto>(s)).ToListAsync();
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
