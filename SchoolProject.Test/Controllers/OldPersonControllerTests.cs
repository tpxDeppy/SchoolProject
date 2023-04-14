using AutoMapper;
using FluentValidation;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Data;
using Moq.EntityFrameworkCore;
using SchoolProject.Models.Entities.Enums;
using SchoolProject.Services.Implementations;
using SchoolProject.Models.Entities;
using FluentValidation.Results;
using SchoolProject.Services.Interfaces;

namespace SchoolProject.Tests.Controllers
{
    public class OldPersonControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IValidator<Person>> _validatorMock = new();
        private readonly Mock<IPersonService> _personServiceMock;

        public OldPersonControllerTests()
        {
            var personService = new PersonService(_dataContextMock.Object, _mapperMock.Object, _validatorMock.Object);
            _personServiceMock = new Mock<IPersonService>();
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
            _personServiceMock.Setup(p => p.GetAllPeople())
                .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                {
                    Success = true,
                    Data = dbPeople.Select(p => new GetPersonDto
                    {
                        UserID = p.UserID,
                        LastName = p.LastName
                    }).ToList()
                });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID, LastName = p.LastName });

            //Act
            var result = await _personServiceMock.Object.GetAllPeople();

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
            _personServiceMock.Setup(p => p.GetAllPeople())
                .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                {
                    Success = false,
                    Data = dbPeople.Select(p => new GetPersonDto()).ToList(),
                    Message = "Could not find any data..."
                });

            DataMockSetup(dbPeople);

            //Act
            var result = await _personServiceMock.Object.GetAllPeople();

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

            _personServiceMock.Setup(p => p.GetPersonById(It.IsAny<Guid>()))
                   .ReturnsAsync(new ServiceResponse<GetPersonDto>
                   {
                       Success = true,
                       Data = new GetPersonDto { UserID = dbPeople[0].UserID }
                   });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

            //Act
            var result = await _personServiceMock.Object.GetPersonById(dbPeople[0].UserID);

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
            _personServiceMock.Setup(p => p.GetPersonById(It.IsAny<Guid>()))
                   .ReturnsAsync(new ServiceResponse<GetPersonDto>
                   {
                       Success = false,
                       Data = null,
                       Message = $"Could not find a person with ID of '{dbPerson.UserID}'."
                   });

            _dataContextMock.Setup(p => p.Person);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

            //Act
            var result = await _personServiceMock.Object.GetPersonById(dbPerson.UserID);

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
            _personServiceMock.Setup(p => p.GetPersonByLastName(It.IsAny<string>()))
                   .ReturnsAsync(new ServiceResponse<GetPersonDto>
                   {
                       Success = true,
                       Data = new GetPersonDto { LastName = dbPeople[0].LastName }
                   });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { LastName = p.LastName });

            //Act
            var result = await _personServiceMock.Object.GetPersonByLastName(dbPeople[0].LastName);

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
            _personServiceMock.Setup(p => p.GetPersonByLastName(It.IsAny<string>()))
                   .ReturnsAsync(new ServiceResponse<GetPersonDto>
                   {
                       Success = false,
                       Data = null,
                       Message = $"Could not find a person with the last name of '{dbPerson.LastName}'."
                   });

            _dataContextMock.Setup(p => p.Person);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { LastName = p.LastName });

            //Act
            var result = await _personServiceMock.Object.GetPersonByLastName(dbPerson.LastName);

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
            _personServiceMock.Setup(p => p.GetPeopleByUserType(It.IsAny<UserType>()))
                   .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                   {
                       Success = true,
                       Data = new List<GetPersonDto>
                       {
                           new GetPersonDto
                           {
                               UserType = dbPeople[0].UserType
                           }
                       }
                   });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserType = p.UserType });

            //Act
            var result = await _personServiceMock.Object.GetPeopleByUserType(dbPeople[0].UserType);

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
            _personServiceMock.Setup(p => p.GetPeopleByUserType(It.IsAny<UserType>()))
                   .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                   {
                       Success = false,
                       Data = new List<GetPersonDto>()
                   });

            DataMockSetup(dbPeople);

            //Act
            var result = await _personServiceMock.Object.GetPeopleByUserType(UserType.Pupil);

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
            _personServiceMock.Setup(p => p.GetPupilsByYearGroup(It.IsAny<int>()))
                   .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                   {
                       Success = true,
                       Data = new List<GetPersonDto>
                       {
                           new GetPersonDto
                           {
                               UserType = dbPeople[0].UserType,
                               YearGroup = (int)dbPeople[0].YearGroup!
                           }
                       }
                   });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserType = p.UserType, YearGroup = p.YearGroup });

            //Act
            var result = await _personServiceMock.Object.GetPupilsByYearGroup((int)dbPeople[0].YearGroup!);

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
            _personServiceMock.Setup(p => p.GetPupilsByYearGroup(It.IsAny<int>()))
                   .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                   {
                       Success = false,
                       Data = new List<GetPersonDto>()
                   });

            DataMockSetup(dbPeople);

            //Act
            var result = await _personServiceMock.Object.GetPupilsByYearGroup(15);

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
            _personServiceMock.Setup(p => p.GetPeopleFromSchool(It.IsAny<Guid>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Success = true,
                      Data = new List<GetPersonDto>
                       {
                           new GetPersonDto
                           {
                               UserType = dbPeople[0].UserType,
                               YearGroup = dbPeople[0].YearGroup,
                               SchoolID = dbPeople[0].SchoolID
                           },
                           new GetPersonDto
                           {
                               UserType = dbPeople[1].UserType,
                               SchoolID = dbPeople[1].SchoolID
                           }
                       }
                  });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto
                       { UserType = p.UserType, YearGroup = p.YearGroup, SchoolID = p.SchoolID });

            //Act
            var result = await _personServiceMock.Object.GetPeopleFromSchool(Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"));

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
            Guid schoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5");
            _personServiceMock.Setup(p => p.GetPeopleFromSchool(It.IsAny<Guid>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Success = false,
                      Data = new List<GetPersonDto>(),
                      Message = $"Could not find people in the school with ID of '{schoolID}'."
                  });

            DataMockSetup(dbPeople);

            //Act
            var result = await _personServiceMock.Object.GetPeopleFromSchool(schoolID);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetPeopleInClass_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            Guid classID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0");
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
            _personServiceMock.Setup(p => p.GetPeopleInClass(It.IsAny<Guid>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Data = dbPeople.Select(p => new GetPersonDto { UserID = p.UserID }).ToList()
                  });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

            //Act
            var result = await _personServiceMock.Object.GetPeopleInClass(classID);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople.Count, result.Data.Count);
        }

        [Fact]
        public async Task GetPeopleInClass_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            Guid classID = Guid.Parse("9f082281-2925-4261-a339-be2f4db65271");
            var dbPeople = new List<Person>();
            _personServiceMock.Setup(p => p.GetPeopleInClass(It.IsAny<Guid>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Success = false,
                      Data = new List<GetPersonDto>(),
                      Message = $"Could not find people in the class with ID of '{classID}'."
                  });

            DataMockSetup(dbPeople);

            //Act
            var result = await _personServiceMock.Object.GetPeopleInClass(classID);

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
            _personServiceMock.Setup(p => p.GetPeopleInClassByName(It.IsAny<string>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Data = dbPeople.Select(p => new GetPersonDto { UserID = p.UserID }).ToList()
                  });

            DataMockSetup(dbPeople);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(It.IsAny<Person>()))
                       .Returns<Person>(p => new GetPersonDto { UserID = p.UserID });

            //Act
            var result = await _personServiceMock.Object.GetPeopleInClassByName("Acting");

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbPeople.Count, result.Data.Count);
        }

        [Fact]
        public async Task GetPeopleInClassByName_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            string className = "History";
            var dbPeople = new List<Person>();
            _personServiceMock.Setup(p => p.GetPeopleInClassByName(It.IsAny<string>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Success = false,
                      Data = new List<GetPersonDto>(),
                      Message = $"Could not find people in the class with name of '{className}'."
                  });

            DataMockSetup(dbPeople);

            //Act
            var result = await _personServiceMock.Object.GetPeopleInClassByName(className);

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

            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Success = true,
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        UserID = Guid.NewGuid(),
                        FirstName = "John",
                        LastName = "Doe",
                        UserType = UserType.Teacher,
                        SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5") }
                },
                Message = $"Successfully created new person with the first name of '{expectedPerson.FirstName}'."
            };

            _personServiceMock.Setup(p => p.AddPerson(It.IsAny<AddPersonDto>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Data = expectedResponse.Data,
                      Message = expectedResponse.Message
                  });

            _mapperMock.Setup(p => p.Map<Person>(It.IsAny<AddPersonDto>())).Returns(expectedPerson);
            _mapperMock.Setup(p => p.Map<GetPersonDto>(expectedPerson)).Returns(expectedResponse.Data[0]);
            _dataContextMock.Setup(p => p.Person.Add(expectedPerson));
            _dataContextMock.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(1);

            _dataContextMock.Setup(context => context.Person)
                            .ReturnsDbSet(new List<Person> { expectedPerson }.AsQueryable());

            _validatorMock.Setup(v => v.ValidateAsync(expectedPerson, default)).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _personServiceMock.Object.AddPerson(addPersonDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetPersonDto>>>(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Message, result.Message);
            Assert.NotNull(result.Data);
            Assert.Collection(result.Data, item =>
            {
                Assert.Equal(expectedResponse.Data[0].UserID, item.UserID);
                Assert.Equal(expectedResponse.Data[0].FirstName, item.FirstName);
                Assert.Equal(expectedResponse.Data[0].LastName, item.LastName);
                Assert.Equal(expectedResponse.Data[0].UserType, item.UserType);
                Assert.Equal(expectedResponse.Data[0].SchoolID, item.SchoolID);
            });
        }

        [Fact]
        public async Task AddPerson_ReturnsError_WhenPersonIsInvalid()
        {
            // Arrange
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

            _personServiceMock.Setup(p => p.AddPerson(It.IsAny<AddPersonDto>()))
                .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                {
                    Success = false,
                    Data = null,
                    Message = "Validation error. Person was not saved."
                });

            _mapperMock.Setup(x => x.Map<Person>(It.IsAny<AddPersonDto>())).Returns(newPerson);
            _validatorMock.Setup(x => x.Validate(newPerson))
                          .Returns(new ValidationResult(new List<ValidationFailure> {
                          new ValidationFailure("First_name", "First name must be between 3 and 20 characters.")
                          }));

            // Act
            var result = await _personServiceMock.Object.AddPerson(addPersonDto);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

    }
}