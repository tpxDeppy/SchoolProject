using AutoMapper;
using SchoolProject.API.DataTransferObjs.Class;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.API.DataTransferObjs.School;
using SchoolProject.BL.Models;

namespace SchoolProject.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Person, GetPersonDto>();
            CreateMap<AddPersonDto, Person>();
            CreateMap<UpdatePersonDto, Person>();
            CreateMap<School, GetSchoolDto>();
            CreateMap<AddSchoolDto, School>();
            CreateMap<Class, GetClassDto>();
            CreateMap<AddClassDto, Class>();
        }


    }
}
