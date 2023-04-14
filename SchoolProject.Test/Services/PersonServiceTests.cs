using AutoMapper;
using FluentValidation;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Data;
using Moq.EntityFrameworkCore;
using SchoolProject.Models.Entities.Enums;
using SchoolProject.Services.Implementations;
using SchoolProject.Models.Entities;
using FluentValidation.Results;
using SchoolProject.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace SchoolProject.Tests.Services
{
    public class PersonServiceTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IValidator<Person>> _validatorMock = new();
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            var validator = new PersonValidator(_dataContextMock.Object);
            _personService = new PersonService(_dataContextMock.Object, _mapperMock.Object, validator);
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
                    UserID = Guid.Parse("a9efa426-6ec3-490c-bab4-0b49150f9776"),
                    LastName = "Evans"
                }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID, LastName = p.LastName });

            //Act
            var result = await _personService.GetAllPeople();

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople.Count, result.Data.Count);
            Assert.Equal(dbPeople[0].UserID, result.Data[0].UserID);
            Assert.Equal(dbPeople[0].LastName, result.Data[0].LastName);
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
                new Person { UserID = Guid.Parse("730ef62d-fc45-4f6d-8a09-0f99e4316a3a") }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

            //Act
            var result = await _personService.GetPersonById(dbPeople[0].UserID);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].UserID, result.Data.UserID);
        }

        [Fact]
        public async Task GetSinglePerson_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            //Arrange
            var dbPerson = new Person { UserID = Guid.NewGuid() };
            _dataContextMock.Setup(p => p.Person);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

            //Act
            var result = await _personService.GetPersonById(dbPerson.UserID);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPersonByLastName_ReturnsOk_WhenPersonExists()
        {
            //Arrange
            var dbPeople = new List<Person> { new Person { LastName = "Jolie" } };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { LastName = p.LastName });

            //Act
            var result = await _personService.GetPersonByLastName(dbPeople[0].LastName);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].LastName, result.Data.LastName);
        }

        [Fact]
        public async Task GetPersonByLastName_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            //Arrange
            var dbPerson = new Person { LastName = "Julian" };
            _dataContextMock.Setup(p => p.Person);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { LastName = p.LastName });

            //Act
            var result = await _personService.GetPersonByLastName(dbPerson.LastName);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPeopleByUserType_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var dbPeople = new List<Person> { new Person { UserType = UserType.Teacher } };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserType = p.UserType });

            //Act
            var result = await _personService.GetPeopleByUserType(dbPeople[0].UserType);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].UserType, result.Data[0].UserType);
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
                new Person { UserType = UserType.Pupil, YearGroup = 11 }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserType = p.UserType, YearGroup = p.YearGroup });

            //Act
            var result = await _personService.GetPupilsByYearGroup((int)dbPeople[0].YearGroup!);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].UserType, result.Data[0].UserType);
            Assert.Equal(dbPeople[0].YearGroup, result.Data[0].YearGroup);
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
                    UserType = UserType.Pupil, YearGroup = 9,
                    SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d") },
                new Person {
                    UserType = UserType.Teacher,
                    SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d") }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto
                       { UserType = p.UserType, YearGroup = p.YearGroup, SchoolID = p.SchoolID });

            //Act
            var result = await _personService.GetPeopleFromSchool(Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"));

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople[0].UserType, result.Data[0].UserType);
            Assert.Equal(dbPeople[0].YearGroup, result.Data[0].YearGroup);
            Assert.Equal(dbPeople[0].SchoolID, result.Data[0].SchoolID);
            Assert.Equal(dbPeople[1].UserType, result.Data[1].UserType);
            Assert.Equal(dbPeople[1].SchoolID, result.Data[1].SchoolID);
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
                                    ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                                    UserID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc")
                                }
                    }
                },
                new Person
                {
                    PersonClasses = new List<PersonClass>
                    {
                                new PersonClass
                                {
                                    ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                                    UserID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                                }
                    }
                }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

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
                            ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            UserID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"),
                            Class = new Class { ClassName = "Acting" }
                        }
                    }
                },
                new Person
                {
                    PersonClasses = new List<PersonClass>
                    {
                        new PersonClass
                        {
                            ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                            UserID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0"),
                            Class = new Class { ClassName = "Acting" }
                        }
                    }
                }
            };
            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

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
            //Arrange
            var addPersonDto = new AddPersonDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };

            var expectedPerson = new Person
            {
                UserID = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            string expectedMessage = $"Successfully created new person with the first name of '{expectedPerson.FirstName}'.";

            _mapperMock.Setup(p => p.Map<Person>(It.IsAny<AddPersonDto>())).Returns(expectedPerson);

            _validatorMock.Setup(v => v.ValidateAsync(expectedPerson, default))
                          .ReturnsAsync(new ValidationResult());

            _mapperMock.Setup(p => p.Map<GetPersonDto>(expectedPerson))
                       .Returns(new GetPersonDto
                       {
                           FirstName = expectedPerson.FirstName,
                           LastName = expectedPerson.LastName,
                           UserType = expectedPerson.UserType,
                           SchoolID = expectedPerson.SchoolID
                       });

            _dataContextMock.Setup(context => context.Person)                
                            .ReturnsDbSet(new List<Person> { expectedPerson }.AsQueryable());            

            //Act
            var result = await _personService.AddPerson(addPersonDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetPersonDto>>>(result);
            Assert.True(result.Success);
            Assert.Equal(expectedMessage, result.Message);
            Assert.NotNull(result.Data);            
            Assert.Equal(result.Data[0].FirstName, addPersonDto.FirstName);
            Assert.Equal(result.Data[0].LastName, addPersonDto.LastName);
            Assert.Equal(result.Data[0].UserType, addPersonDto.UserType);
            Assert.Equal(result.Data[0].SchoolID, addPersonDto.SchoolID);
        }

        [Fact]
        public async Task AddPerson_ReturnsValidationError_WhenPersonIsInvalid()
        {
            //Arrange
            var addPersonDto = new AddPersonDto
            {
                FirstName = "J",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var newPerson = new Person
            {
                UserID = Guid.NewGuid(),
                FirstName = "J",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };

            _mapperMock.Setup(p => p.Map<Person>(addPersonDto)).Returns(newPerson);

            _validatorMock.Setup(v => v.Validate(newPerson))
                          .Returns(new ValidationResult(new List<ValidationFailure>
                          {
                            new ValidationFailure("First_name", "First name must be between 3 and 20 characters.")
                          }));

            //Act
            var result = await _personService.AddPerson(addPersonDto);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsValidServiceResponseWithUpdatedPerson_WhenValidInput()
        {
            //Arrange
            var updatedPersonDto = new UpdatePersonDto
            {
                UserID = Guid.Parse("f6468ad4-7927-419c-9356-a29a25d894f8"),
                FirstName = "Tom",
                LastName = "Holland",
                UserType = UserType.Pupil,
                DateOfBirth = new DateTime(2010,3,10),
                YearGroup = 6,
                SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d")
            };

            var expectedPerson = new Person
            {
                UserID = Guid.Parse("f6468ad4-7927-419c-9356-a29a25d894f8"),
                FirstName = "Tom",
                LastName = "Holland",
                UserType = UserType.Pupil,
                DateOfBirth = new DateTime(2010, 3, 10),
                YearGroup = 7,
                SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d")
            };

            _mapperMock.Setup(p => p.Map<Person>(updatedPersonDto)).Returns(expectedPerson);

            _validatorMock.Setup(v => v.ValidateAsync(expectedPerson, default))
                          .ReturnsAsync(new ValidationResult());

            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns(new GetPersonDto
                        {
                            UserID = expectedPerson.UserID,
                            FirstName = expectedPerson.FirstName,
                            LastName = expectedPerson.LastName,
                            UserType = expectedPerson.UserType,
                            DateOfBirth = expectedPerson.DateOfBirth,
                            YearGroup = expectedPerson.YearGroup,
                            SchoolID = expectedPerson.SchoolID
                        });

            _dataContextMock.Setup(context => context.Person)
                            .ReturnsDbSet(new List<Person> { expectedPerson }.AsQueryable());            

            //Act
            var result = await _personService.UpdatePerson(updatedPersonDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<GetPersonDto>>(result);
            Assert.True(result.Success);
            Assert.Equal("Successfully updated.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(updatedPersonDto.UserID, result.Data.UserID);
            Assert.Equal(updatedPersonDto.FirstName, result.Data.FirstName);
            Assert.Equal(updatedPersonDto.LastName, result.Data.LastName);
            Assert.Equal(updatedPersonDto.UserType, result.Data.UserType);
            Assert.Equal(updatedPersonDto.SchoolID, result.Data.SchoolID);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsValidationError_WhenInputIsInvalid()
        {
            //Arrange
            var updatedPersonDto = new UpdatePersonDto
            {
                UserID = Guid.Parse("f6468ad4-7927-419c-9356-a29a25d894f8"),
                FirstName = "Tom",
                LastName = "Holland",
                UserType = UserType.Pupil,
                DateOfBirth = new DateTime(2010, 3, 10),
                YearGroup = 6,
                SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d")
            };

            var expectedPerson = new Person
            {
                UserID = Guid.Parse("f6468ad4-7927-419c-9356-a29a25d894f8"),
                FirstName = "Tom",
                LastName = "Holland",
                UserType = UserType.Pupil,
                DateOfBirth = new DateTime(2019, 3, 10),
                YearGroup = 6,
                SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d")
            };

            _mapperMock.Setup(p => p.Map<Person>(updatedPersonDto)).Returns(expectedPerson);

            _validatorMock.Setup(v => v.Validate(expectedPerson))
                          .Returns(new ValidationResult(new List<ValidationFailure>
                          {
                            new ValidationFailure("Date_of_birth", 
                            "Age of pupil must be between 5 and 18 years old. Please enter a valid date of birth.")
                          }));

            //Act
            var result = await _personService.UpdatePerson(updatedPersonDto);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task DeletePerson_ReturnsValidServiceResponse_WhenValidInput()
        {
            //Arrange
            Guid personID = Guid.Parse("a9efa426-6ec3-490c-bab4-0b49150f9776");
            var dbPeople = new List<Person>
            {
                new Person
                {
                    UserID = personID,
                    FirstName = "Chris",
                    LastName = "Evans",
                    UserType = UserType.Teacher,
                    SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d")
                }
            };

            DataMockSetup(dbPeople);            
            _dataContextMock.Setup(p => p.Person.FindAsync(personID))
                            .ReturnsAsync(dbPeople[0]);

            _mapperMock.Setup(p => p.Map<GetPersonDto>(dbPeople[0]))
                       .Returns(new GetPersonDto
                       {
                           UserID = dbPeople[0].UserID,
                           FirstName = dbPeople[0].FirstName,
                           LastName = dbPeople[0].LastName,
                           UserType = dbPeople[0].UserType,
                           SchoolID = dbPeople[0].SchoolID
                       });

            //Act
            var result = await _personService.DeletePerson(personID);

            //Assert
            Assert.IsType<ServiceResponse<List<GetPersonDto>>>(result);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data[0]);
            Assert.True(result.Success);
            Assert.Empty(result.Message);
        }

        [Fact]
        public async Task DeletePerson_ReturnsNotFoundMessage_WhenInputIsInvalid()
        {
            //Arrange
            Guid personID = Guid.NewGuid();
            var dbPeople = new List<Person>
            {
                new Person
                {
                    UserID = Guid.Parse("a9efa426-6ec3-490c-bab4-0b49150f9776"),
                    FirstName = "Chris",
                    LastName = "Evans",
                    UserType = UserType.Teacher,
                    SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d")
                }
            };

            DataMockSetup(dbPeople);
            _dataContextMock.Setup(p => p.Person.FindAsync(personID))
                            .ReturnsAsync(dbPeople[0]);

            //Act
            var result = await _personService.DeletePerson(personID);

            //Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);            
            Assert.Equal($"Person with ID '{personID}' could not be found.", result.Message);
        }
    }
}