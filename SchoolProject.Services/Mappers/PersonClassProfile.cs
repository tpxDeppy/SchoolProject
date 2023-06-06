using AutoMapper;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Models.Entities;

namespace SchoolProject.Services.Mappers
{
    public class PersonClassProfile : Profile
    {
        public PersonClassProfile()
        {
            CreateMap<PersonClass, Class>().ReverseMap();
            CreateMap<PersonClassDto, PersonClass>();
            CreateMap<PersonClassDto, Person>();
        }
    }
}
