using AutoMapper;
using FluentValidation;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Data;
using Moq.EntityFrameworkCore;
using SchoolProject.Models.Entities.Enums;
using SchoolProject.Services.Implementations;
using SchoolProject.Models.Entities;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Controllers;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;

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

        private void DataMockSetup(List<Person> people)
        {
            _dataContextMock.Setup(p => p.Person).ReturnsDbSet(people);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person> 
            { 
                new Person 
                { 
                    User_ID = Guid.Parse("a9efa426-6ec3-490c-bab4-0b49150f9776"),
                    Last_name = "Evans"
                }
            };
            DataMockSetup(dbPeople);
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
            DataMockSetup(dbPeople);

            //Act
            var result = await _personService.GetAllPeople();

            //Assert
            Assert.False(result.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetSinglePerson_ReturnsOk_WhenPersonExists()
        {
            //Arrange
            var dbPeople = new List<Person> 
            { 
                new Person { User_ID = Guid.Parse("730ef62d-fc45-4f6d-8a09-0f99e4316a3a") } 
            };
            DataMockSetup(dbPeople);            
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
            var dbPerson = new Person { User_ID = Guid.NewGuid() };
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
            DataMockSetup(dbPeople);
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
            DataMockSetup(dbPeople);
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
            DataMockSetup(dbPeople);

            //Act
            var result = await _personService.GetPeopleByUserType(UserType.Pupil);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.Empty(result.Message);
        }

        [Fact]
        public async Task GetPupilsByYearGroup_ReturnsOk_WhenPupilsExist()
        {
            //Arrange
            var dbPeople = new List<Person> 
            { 
                new Person { User_type = UserType.Pupil, Year_group = 11 } 
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_type = p.User_type, Year_group = p.Year_group });

            //Act
            var result = await _personService.GetPupilsByYearGroup((int)dbPeople[0].Year_group!);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].User_type, result.Data[0].User_type);
            Assert.Equal(dbPeople[0].Year_group, result.Data[0].Year_group);
        }

        [Fact]
        public async Task GetPupilsByYearGroup_ReturnsNotFound_WhenPupilsDoNotExist()
        {
            //Arrange
            var dbPeople = new List<Person>();
            DataMockSetup(dbPeople);

            //Act
            var result = await _personService.GetPupilsByYearGroup(15);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.Empty(result.Message);
        }

        [Fact]
        public async Task GetPeopleFromSchool_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person> 
            { 
                new Person { 
                    User_type = UserType.Pupil, Year_group = 9, 
                    School_ID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d") },
                new Person { 
                    User_type = UserType.Teacher, 
                    School_ID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d") } 
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto 
                                            { User_type = p.User_type, Year_group = p.Year_group, School_ID = p.School_ID });

            //Act
            var result = await _personService.GetPeopleFromSchool(Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"));

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].User_type, result.Data[0].User_type);
            Assert.Equal(dbPeople[0].Year_group, result.Data[0].Year_group);
            Assert.Equal(dbPeople[0].School_ID, result.Data[0].School_ID);
            Assert.Equal(dbPeople[1].User_type, result.Data[1].User_type);
            Assert.Equal(dbPeople[1].School_ID, result.Data[1].School_ID);
        }

        [Fact]
        public async Task GetPeopleFromSchool_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            var dbPeople = new List<Person>();
            DataMockSetup(dbPeople);

            //Act
            var result = await _personService.GetPeopleFromSchool(Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5"));

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPeopleInClass_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person>
            {
                new Person
                {
                    PersonClasses = new List<PersonClass>
                    {
                                new PersonClass
                                { 
                                    Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                                    User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc") 
                                } 
                    } 
                },
                new Person
                {
                    PersonClasses = new List<PersonClass>
                    {
                                new PersonClass
                                { 
                                    Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                                    User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0") 
                                } 
                    } 
                }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_ID = p.User_ID });

            //Act
            var result = await _personService.GetPeopleInClass(Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"));

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople.Count, result.Data.Count);
        }

        [Fact]
        public async Task GetPeopleInClass_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            var dbPeople = new List<Person>();
            DataMockSetup(dbPeople);

            //Act
            var result = await _personService.GetPeopleInClass(Guid.Parse("9f082281-2925-4261-a339-be2f4db65271"));

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPeopleInClassByName_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person>
            {
                new Person
                {
                    PersonClasses = new List<PersonClass>
                    {
                        new PersonClass
                        { 
                            Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            User_ID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"),
                            Class = new Class { Class_name = "Acting" }
                        } 
                    } 
                },
                new Person
                {
                    PersonClasses = new List<PersonClass> 
                    {
                        new PersonClass
                        {
                            Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            User_ID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0"),
                            Class = new Class { Class_name = "Acting" }
                        } 
                    } 
                }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { User_ID = p.User_ID });

            //Act
            var result = await _personService.GetPeopleInClassByName("Acting");

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople.Count, result.Data.Count);
        }

        [Fact]
        public async Task GetPeopleInClassByName_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            var dbPeople = new List<Person>();
            DataMockSetup(dbPeople);

            //Act
            var result = await _personService.GetPeopleInClassByName("History");

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task AddPerson_ReturnsValidServiceResponseWithAddedPerson_WhenValidInput()
        {
            // Arrange
            var addPersonDto = new AddPersonDto
            {
                First_name = "John",
                Last_name = "Doe",
                User_type = UserType.Teacher,
                School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };

            var expectedPerson = new Person
            {
                User_ID = Guid.NewGuid(),
                First_name = "John",
                Last_name = "Doe",
                User_type = UserType.Teacher,
                School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };

            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Success = true,
                Message = $"Successfully created new person with the first name of '{expectedPerson.First_name}'.",
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        User_ID = Guid.NewGuid(),
                        First_name = "John",
                        Last_name = "Doe",
                        User_type = UserType.Teacher,
                        School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5") }
                }
            };
            
            _mapperMock.Setup(p => p.Map<Person>(addPersonDto)).Returns(expectedPerson);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(expectedPerson)).Returns(expectedResponse.Data[0]);
            _dataContextMock.Setup(p => p.Person.Add(expectedPerson));
            _dataContextMock.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(1);

            _dataContextMock.Setup(context => context.Person)
                            .ReturnsDbSet(new List<Person> { expectedPerson }.AsQueryable());

            _validatorMock.Setup(v => v.ValidateAsync(expectedPerson, default)).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _personService.AddPerson(addPersonDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetPersonDto>>>(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Message, result.Message);
            Assert.NotNull(result.Data);
            Assert.Collection(result.Data, item =>
            {
                Assert.Equal(expectedResponse.Data[0].User_ID, item.User_ID);
                Assert.Equal(expectedResponse.Data[0].First_name, item.First_name);
                Assert.Equal(expectedResponse.Data[0].Last_name, item.Last_name);
                Assert.Equal(expectedResponse.Data[0].User_type, item.User_type);
                Assert.Equal(expectedResponse.Data[0].School_ID, item.School_ID);
            });
        }

        //[Fact]
        //public async Task AddPerson_ReturnsValididationError_WhenInvalidInput()
        //{
        //    // Arrange
        //    var addPersonDto = new AddPersonDto
        //    {
        //        First_name = "J",
        //        Last_name = "Doe",
        //        User_type = UserType.Teacher,
        //        School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
        //    };

        //    //var expectedPerson = new Person
        //    //{
        //    //    User_ID = Guid.NewGuid(),
        //    //    First_name = "John",
        //    //    Last_name = "Doe",
        //    //    User_type = UserType.Teacher,
        //    //    School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
        //    //};

        //    var expectedResponse = new ServiceResponse<List<GetPersonDto>>
        //    {
        //        Success = false,
        //        Message = $"Validation error. Person was not saved."
        //    };

        //    _mapperMock.Setup(p => p.Map<Person>(addPersonDto)).Returns(expectedPerson);
        //    _mapperMock.Setup(p => p.Map<GetPersonDto>(expectedPerson)).Returns(expectedResponse.Data[0]);
        //    _dataContextMock.Setup(p => p.Person.Add(expectedPerson));
        //    _dataContextMock.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(1);

        //    _dataContextMock.Setup(context => context.Person)
        //                    .ReturnsDbSet(new List<Person> { expectedPerson }.AsQueryable());

        //    _validatorMock.Setup(v => v.ValidateAsync(expectedPerson, default)).ReturnsAsync(new ValidationResult());

        //    // Act
        //    var result = await _personService.AddPerson(addPersonDto);

        //    // Assert
        //    Assert.Null(result);
        //    Assert.IsType<ServiceResponse<List<GetPersonDto>>>(result);
        //    Assert.False(result.Success);
        //    Assert.Equal(expectedResponse.Message, result.Message);
        //    Assert.Null(result.Data);
        //    //Assert.Collection(result.Data, item =>
        //    //{
        //    //    Assert.Equal(expectedResponse.Data[0].User_ID, item.User_ID);
        //    //    Assert.Equal(expectedResponse.Data[0].First_name, item.First_name);
        //    //    Assert.Equal(expectedResponse.Data[0].Last_name, item.Last_name);
        //    //    Assert.Equal(expectedResponse.Data[0].User_type, item.User_type);
        //    //    Assert.Equal(expectedResponse.Data[0].School_ID, item.School_ID);
        //    //});
        //}







        [Fact]
        public async Task AddPerson_ReturnsError_WhenPersonIsInvalid()
        {
            // Arrange
            var addPersonDto = new AddPersonDto
            {
                First_name = "J",
                Last_name = "Doe",
                User_type = UserType.Teacher,
                School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var newPerson = new Person
            {
                User_ID = Guid.NewGuid(),
                First_name = "J",
                Last_name = "Doe",
                User_type = UserType.Teacher,
                School_ID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            _mapperMock.Setup(x => x.Map<Person>(addPersonDto)).Returns(newPerson);
            _validatorMock.Setup(x => x.Validate(newPerson))
                          .Returns(new ValidationResult(new List<ValidationFailure> {
                          new ValidationFailure("First_name", "First name must be between 3 and 20 characters.")
                          }));

            // Act
            var result = await _personService.AddPerson(addPersonDto);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        
    }
    }