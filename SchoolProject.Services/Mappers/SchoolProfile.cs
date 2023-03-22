using AutoMapper;
using SchoolProject.Models.DataTransferObjs.School;
using SchoolProject.Models.Entities;

namespace SchoolProject.Services.Mappers
{
    public class SchoolProfile : Profile
    {
        public SchoolProfile()
        {
            CreateMap<School, GetSchoolDto>();
            CreateMap<AddSchoolDto, School>();
        }


    }
}
