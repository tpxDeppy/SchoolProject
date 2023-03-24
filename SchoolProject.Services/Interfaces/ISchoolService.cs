using SchoolProject.Models.DataTransferObjs.School;
using SchoolProject.Models.Entities;

namespace SchoolProject.Services.Interfaces
{
    public interface ISchoolService
    {
        Task<ServiceResponse<List<GetSchoolDto>>> GetSchools();
        Task<ServiceResponse<GetSchoolDto>> GetSchoolById(Guid id);
        Task<ServiceResponse<GetSchoolDto>> GetSchoolByName(string schoolName);
        Task<ServiceResponse<List<GetSchoolDto>>> AddSchool(AddSchoolDto newSchool);
        Task<ServiceResponse<GetSchoolDto>> UpdateSchool(UpdateSchoolDto updatedSchool);
        Task<ServiceResponse<List<GetSchoolDto>>> DeleteSchool(Guid id);
    }
}
