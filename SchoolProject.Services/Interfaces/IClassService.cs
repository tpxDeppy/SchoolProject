using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Models.Entities;

namespace SchoolProject.Services.Interfaces
{
    public interface IClassService
    {
        Task<ServiceResponse<List<GetClassDto>>> GetAllClasses();
        Task<ServiceResponse<GetClassDto>> GetClassById(Guid id);
        Task<ServiceResponse<GetClassDto>> GetClassByName(string className);
        Task<ServiceResponse<List<GetClassDto>>> AddClass(AddClassDto newClass);
        Task<ServiceResponse<GetClassDto>> UpdateClass(UpdateClassDto updatedClass);
        Task<ServiceResponse<List<GetClassDto>>> DeleteClass(Guid id);
    }
}
