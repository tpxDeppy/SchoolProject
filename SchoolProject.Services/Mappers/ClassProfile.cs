using AutoMapper;
using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Models.Entities;

namespace SchoolProject.Services.Mappers
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, GetClassDto>();
            CreateMap<AddClassDto, Class>();
        }
    }
}
