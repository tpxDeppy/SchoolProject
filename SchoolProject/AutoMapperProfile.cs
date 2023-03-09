using AutoMapper;
using SchoolProject.API.DataTransferObjs.Person;
using SchoolProject.BL.Models;

namespace SchoolProject.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Person, GetPersonDto>();
            CreateMap<AddPersonDto, Person>();
        }


    }
}
