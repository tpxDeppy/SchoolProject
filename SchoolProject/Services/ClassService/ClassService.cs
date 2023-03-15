using AutoMapper;
using SchoolProject.API.DataTransferObjs.Class;
using SchoolProject.BL.Models;

namespace SchoolProject.API.Services.ClassService
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

                if (dbClasses is null)
                {
                    throw new Exception($"Could not find any data...");
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
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.Class_ID == id);
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
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.Class_name == className);
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

            serviceResponse.Data = 
                await _dataContext.Class.Select(c => _mapper.Map<GetClassDto>(c)).ToListAsync();
 
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetClassDto>> UpdateClass(UpdateClassDto updatedClass)
        {
            var serviceResponse = new ServiceResponse<GetClassDto>();

            try
            {
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.Class_ID == updatedClass.Class_ID);

                if (dbClass is null)
                {
                    throw new Exception($"Class with ID of '{updatedClass.Class_ID}' could not be found.");
                }

                dbClass.Class_name = updatedClass.Class_name;
                dbClass.Class_description = updatedClass.Class_description;

                await _dataContext.SaveChangesAsync();

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
                var dbClass = await _dataContext.Class.FirstOrDefaultAsync(c => c.Class_ID == id);

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
