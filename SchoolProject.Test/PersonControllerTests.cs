using AutoMapper;
using FluentValidation;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Data;
using Moq.EntityFrameworkCore;
using SchoolProject.Models.Entities.Enums;
using SchoolProject.Services.Implementations;
using SchoolProject.Models.Entities;

namespace SchoolProject.Tests
{
    public class PersonControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IValidator<Person>> _validatorMock = new();
        private readonly PersonService _personService;

        public PersonControllerTests()
        {
            _personService = new PersonService(_dataContextMock.Object, _mapperMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person> { new Person { User_ID = Guid.Parse("a9efa426-6ec3-490c-bab4-0b49150f9776"), Last_name = "Evans" } };
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_ID = p.User_ID, Last_name = p.Last_name });

            //Act
            var result = await _personService.GetAllPeople();

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople.Count, result.Data.Count);
            Assert.Equal(dbPeople[0].User_ID, result.Data[0].User_ID);
            Assert.Equal(dbPeople[0].Last_name, result.Data[0].Last_name);
        }

        [Fact]
        public async Task GetAllPeople_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            var dbPeople = new List<Person>();
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(dbPeople);

            //Act
            var result = await _personService.GetAllPeople();

            //Assert
            Assert.False(result.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPersonById_ReturnsOk_WhenPersonExists()
        {
            //Arrange
            var dbPeople = new List<Person> { new Person { User_ID = Guid.Parse("730ef62d-fc45-4f6d-8a09-0f99e4316a3a") } };
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_ID = p.User_ID });
       
            //Act
            var result = await _personService.GetPersonById(dbPeople[0].User_ID);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].User_ID, result.Data.User_ID);
        }

        [Fact]
        public async Task GetSinglePerson_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            //Arrange
            var dbPerson = new Person { User_ID = Guid.Parse("730ef62d-fc45-4f6d-8a09-0f99e4316a3b") };
            _dataContextMock.Setup(p => p.Person);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_ID = p.User_ID });

            //Act
            var result = await _personService.GetPersonById(dbPerson.User_ID);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPersonByLastName_ReturnsOk_WhenPersonExists()
        {
            //Arrange
            var dbPeople = new List<Person> { new Person { Last_name = "Jolie" } };
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { Last_name = p.Last_name});

            //Act
            var result = await _personService.GetPersonByLastName(dbPeople[0].Last_name);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].Last_name, result.Data.Last_name);
        }

        [Fact]
        public async Task GetPersonByLastName_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            //Arrange
            var dbPerson = new Person { Last_name = "Julian" };
            _dataContextMock.Setup(p => p.Person);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { Last_name = p.Last_name });

            //Act
            var result = await _personService.GetPersonByLastName(dbPerson.Last_name);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPeopleByUserType_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person> { new Person { User_type = UserType.Teacher } };
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_type = p.User_type });

            //Act
            var result = await _personService.GetPeopleByUserType(dbPeople[0].User_type);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].User_type, result.Data[0].User_type);
        }

        [Fact]
        public async Task GetPeopleByUserType_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            var dbPeople = new List<Person>();
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(dbPeople);
            //_mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
            //           .Returns<Person>(p => new GetPersonDto { User_type = p.User_type });

            //Act
            var result = await _personService.GetPeopleByUserType(UserType.Pupil);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.Empty(result.Message);
        }
    }
}