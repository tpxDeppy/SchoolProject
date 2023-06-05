using AutoMapper;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Models.Entities;

namespace SchoolProject.Services.Mappers
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, GetPersonDto>();
            CreateMap<AddPersonDto, Person>();
            CreateMap<UpdatePersonDto, Person>();           
        }

    }
}
