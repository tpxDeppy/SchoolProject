﻿using AutoMapper;
using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Data;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SchoolProject.Services.Implementations
{
    public class ClassService : IClassService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ClassService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetClassDto>>> GetAllClasses()
        {
            var serviceResponse = new ServiceResponse<List<GetClassDto>>();

            try
            {
                var dbClasses = await _dataContext.Class.ToListAsync();

                serviceResponse.Data = dbClasses.Select(_mapper.Map<GetClassDto>).ToList();

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

        public async Task<ServiceResponse<GetClassDto>> GetClassById(Guid id)
        {
            var serviceResponse = new ServiceResponse<GetClassDto>();

            try
            {
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.ClassID == id);
                serviceResponse.Data = _mapper.Map<GetClassDto>(dbClass);

                if (dbClass is null)
                {
                    throw new Exception($"Class with ID of '{id}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetClassDto>> GetClassByName(string className)
        {
            var serviceResponse = new ServiceResponse<GetClassDto>();

            try
            {
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.ClassName == className);
                serviceResponse.Data = _mapper.Map<GetClassDto>(dbClass);

                if (dbClass is null)
                {
                    throw new Exception($"Class with the name of '{className}' could not be found.");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetClassDto>>> AddClass(AddClassDto newClass)
        {
            var serviceResponse = new ServiceResponse<List<GetClassDto>>();
            var nClass = _mapper.Map<Class>(newClass);

            _dataContext.Class.Add(nClass);
            await _dataContext.SaveChangesAsync();

            serviceResponse.Message = $"Successfully created a class with the name of '{newClass.ClassName}'.";
            serviceResponse.Data =
                await _dataContext.Class.Select(c => _mapper.Map<GetClassDto>(c)).ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetClassDto>> UpdateClass(UpdateClassDto updatedClass)
        {
            var serviceResponse = new ServiceResponse<GetClassDto>();

            try
            {
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.ClassID == updatedClass.ClassID);

                if (dbClass is null)
                {
                    throw new Exception($"Class with ID of '{updatedClass.ClassID}' could not be found.");
                }

                dbClass = _mapper.Map(updatedClass, dbClass);

                await _dataContext.SaveChangesAsync();

                serviceResponse.Message = "Successfully updated.";
                serviceResponse.Data = _mapper.Map<GetClassDto>(dbClass);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetClassDto>>> DeleteClass(Guid id)
        {
            var serviceResponse = new ServiceResponse<List<GetClassDto>>();

            try
            {
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.ClassID == id);

                if (dbClass is null)
                {
                    throw new Exception($"Class with ID of '{id}' could not be found.");
                }

                _dataContext.Class.Remove(dbClass);

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data =
                    await _dataContext.Class.Select(c => _mapper.Map<GetClassDto>(c)).ToListAsync();

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
