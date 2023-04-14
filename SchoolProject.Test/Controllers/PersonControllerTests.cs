using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Controllers;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.Person;
using SchoolProject.Models.Entities;
using SchoolProject.Models.Entities.Enums;
using SchoolProject.Services.Implementations;
using SchoolProject.Services.Interfaces;

namespace SchoolProject.Tests.Controllers
{
    public class PersonControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IValidator<Person>> _validatorMock = new();
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly Person examplePerson = new Person
        {
            UserID = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            UserType = UserType.Teacher,
            SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
        };

        public PersonControllerTests()
        {
            var personService = new PersonService(_dataContextMock.Object, _mapperMock.Object, _validatorMock.Object);
            _personServiceMock = new Mock<IPersonService>();
        }

        private PersonController PController()
        {
            return new PersonController(_personServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        UserID = Guid.Parse("a9efa426-6ec3-490c-bab4-0b49150f9776"),
                        LastName = "Evans"
                    },
                    new GetPersonDto
                    {
                        UserID = Guid.Parse("f6468ad4-7927-419c-9356-a29a25d894f8"),
                        LastName = "Holland"
                    }
                }
            };
            _personServiceMock.Setup(service => service.GetAllPeople()).ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetAll();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = null,
                Message = "Could not find any data..."
            };
            _personServiceMock.Setup(p => p.GetAllPeople())
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetAll();

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetSinglePerson_ReturnsOk_WhenPersonExists()
        {
            //Arrange
            Guid personID = Guid.Parse("730ef62d-fc45-4f6d-8a09-0f99e4316a3a");
            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Data = new GetPersonDto
                {
                    UserID = personID
                }
            };
            _personServiceMock.Setup(p => p.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetSinglePerson(personID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetPersonDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetSinglePerson_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            //Arrange
            Guid personID = Guid.NewGuid();
            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Data = null,
                Message = $"Could not find a person with ID of '{personID}'."
            };
            _personServiceMock.Setup(p => p.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetSinglePerson(personID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetPersonByLastName_ReturnsOk_WhenPersonExists()
        {
            //Arrange
            string lastName = "Jolie";
            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Data = new GetPersonDto { LastName = lastName }
            };
            _personServiceMock.Setup(p => p.GetPersonByLastName(It.IsAny<string>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPersonByLastName(lastName);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetPersonDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetPersonByLastName_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            //Arrange
            string lastName = "Bob";
            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Data = null,
                Message = $"Could not find a person with the name of '{lastName}'."
            };
            _personServiceMock.Setup(p => p.GetPersonByLastName(It.IsAny<string>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPersonByLastName(lastName);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetPersonByUserType_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            UserType userType = UserType.Teacher;
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        UserType = userType
                    }
                }
            };
            _personServiceMock.Setup(p => p.GetPeopleByUserType(It.IsAny<UserType>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPersonByUserType(userType);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetPersonByUserType_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            UserType userType = UserType.Pupil;
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = null,
                Message = string.Empty
            };
            _personServiceMock.Setup(p => p.GetPeopleByUserType(It.IsAny<UserType>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPersonByUserType(userType);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetPupilsByYearGroup_ReturnsOk_WhenPupilsExist()
        {
            //Arrange
            int yearGroup = 11;
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        UserType = UserType.Pupil,
                        YearGroup = yearGroup
                    }
                }
            };
            _personServiceMock.Setup(p => p.GetPupilsByYearGroup(It.IsAny<int>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPupilsByYearGroup(yearGroup);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetPupilsByYearGroup_ReturnsNotFound_WhenPupilsDoNotExist()
        {
            //Arrange
            int yearGroup = 15;
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = null,
                Message = $"Could not find pupils in this '{yearGroup}' year group."
            };
            _personServiceMock.Setup(p => p.GetPupilsByYearGroup(It.IsAny<int>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPupilsByYearGroup(yearGroup);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetPeopleFromSchool_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d");
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        UserType = UserType.Pupil,
                        YearGroup = 9,
                        SchoolID = schoolID
                    },
                    new GetPersonDto
                    {
                        UserType = UserType.Teacher,
                        SchoolID = schoolID
                    }
                }
            };
            _personServiceMock.Setup(p => p.GetPeopleFromSchool(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPeopleFromSchool(schoolID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetPeopleFromSchool_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            Guid schoolID = Guid.NewGuid();
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = null,
                Message = $"Could not find people in the school with ID of '{schoolID}'."
            };
            _personServiceMock.Setup(p => p.GetPeopleFromSchool(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPeopleFromSchool(schoolID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
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
                            ClassID = classID,
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
                            ClassID = classID,
                            UserID = Guid.Parse("4ca1789c-b20c-4320-8472-c52ebeac47e0")
                        }
                    }
                }
            };
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = dbPeople.Select(p => new GetPersonDto { UserID = p.UserID }).ToList()
            };
            _personServiceMock.Setup(p => p.GetPeopleInClass(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPeopleInClass(classID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetPeopleInClass_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            Guid classID = Guid.Parse("9f082281-2925-4261-a339-be2f4db65271");
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = null,
                Message = $"Could not find people in the class with ID of '{classID}'."
            };
            _personServiceMock.Setup(p => p.GetPeopleInClass(It.IsAny<Guid>()))
                   .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPeopleInClass(classID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetPeopleInClassByName_ReturnsOk_WhenPeopleExist()
        {
            //Arrange
            string className = "Acting";
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
                            Class = new Class { ClassName = className }
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
                            Class = new Class { ClassName = className }
                        }
                    }
                }
            };
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = dbPeople.Select(p => new GetPersonDto { UserID = p.UserID }).ToList()
            };
            _personServiceMock.Setup(p => p.GetPeopleInClassByName(It.IsAny<string>()))
                  .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPeopleInClassByName(className);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetPeopleInClassByName_ReturnsNotFound_WhenPeopleDoNotExist()
        {
            //Arrange
            string className = "History";
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Data = null,
                Message = $"Could not find people in the class with the name of '{className}'."
            };
            _personServiceMock.Setup(p => p.GetPeopleInClassByName(It.IsAny<string>()))
                       .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().GetPeopleInClassByName(className);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task AddPerson_ReturnsCreatedWithAddedPerson_WhenValidInput()
        {
            //Arrange
            var addPersonDto = new AddPersonDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var expectedPerson = examplePerson;
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Success = true,
                Data = new List<GetPersonDto>
                {
                    new GetPersonDto
                    {
                        UserID = Guid.NewGuid(),
                        FirstName = examplePerson.FirstName,
                        LastName = examplePerson.LastName,
                        UserType = examplePerson.UserType,
                        SchoolID = examplePerson.SchoolID
                    }
                },
                Message = $"Successfully created new person with the first name of '{expectedPerson.FirstName}'."
            };

            _personServiceMock.Setup(p => p.AddPerson(It.IsAny<AddPersonDto>()))
                  .ReturnsAsync(new ServiceResponse<List<GetPersonDto>>
                  {
                      Data = expectedResponse.Data,
                      Message = expectedResponse.Message
                  });

            //Act
            var result = await PController().AddPerson(addPersonDto);

            //Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var resultData = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(createdResult.Value);
            Assert.True(resultData.Success);
            Assert.Equal(expectedResponse.Message, resultData.Message);
            Assert.Equal(expectedPerson.FirstName, resultData.Data![0].FirstName);
            Assert.Equal(expectedPerson.LastName, resultData.Data[0].LastName);
            Assert.Equal(expectedPerson.UserType, resultData.Data[0].UserType);
            Assert.Equal(expectedPerson.SchoolID, resultData.Data[0].SchoolID);
        }

        [Fact]
        public async Task AddPerson_ReturnsError_WhenPersonIsInvalid()
        {
            //Arrange
            var addPersonDto = new AddPersonDto
            {
                FirstName = "J",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Message = "Validation error. Person was not saved."
            };

            _personServiceMock.Setup(p => p.AddPerson(It.IsAny<AddPersonDto>()))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().AddPerson(addPersonDto);

            //Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var errorMessage = Assert.IsType<ServiceResponse<List<GetPersonDto>>>(createdResult.Value);
            Assert.Equal(expectedResponse.Message, errorMessage.Message);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsOkWithUpdatedPerson_WhenValidInput()
        {
            //Arrange
            Guid personID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc");
            var updatedPersonDto = new UpdatePersonDto
            {
                UserID = personID,
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var expectedPerson = examplePerson;
            expectedPerson.UserID = personID;
            expectedPerson.FirstName = "Joanne";

            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Success = true,
                Data = new GetPersonDto
                {
                    UserID = personID,
                    FirstName = "Joanne",
                    LastName = "Doe",
                    UserType = UserType.Teacher,
                    SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
                },                
                Message = "Successfully updated."
            };

            _personServiceMock.Setup(p => p.UpdatePerson(updatedPersonDto))
                  .ReturnsAsync(new ServiceResponse<GetPersonDto>
                  {
                      Data = expectedResponse.Data,
                      Message = expectedResponse.Message
                  });

            //Act
            var result = await PController().UpdatePerson(personID, updatedPersonDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultData = Assert.IsType<ServiceResponse<GetPersonDto>>(okResult.Value);
            Assert.True(resultData.Success);
            Assert.Equal(expectedResponse.Message, resultData.Message);
            Assert.Equal(expectedPerson.FirstName, resultData.Data!.FirstName);
            Assert.Equal(expectedPerson.LastName, resultData.Data.LastName);
            Assert.Equal(expectedPerson.UserType, resultData.Data.UserType);
            Assert.Equal(expectedPerson.SchoolID, resultData.Data.SchoolID);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsBadRequest_WhenPersonIDsDontMatch()
        {
            //Arrange
            Guid personID = Guid.NewGuid();
            var updatedPersonDto = new UpdatePersonDto
            {
                UserID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc"),
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Message = "Bad request. Please check that the IDs match."
            };

            _personServiceMock.Setup(p => p.UpdatePerson(updatedPersonDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().UpdatePerson(personID, updatedPersonDto);
                      
            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePerson_ReturnsNotFound_WhenPersonIDIsInvalid()
        {
            //Arrange
            Guid personID = Guid.NewGuid();
            var updatedPersonDto = new UpdatePersonDto
            {
                UserID = personID,
                FirstName = "John",
                LastName = "Doe",
                UserType = UserType.Teacher,
                SchoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5")
            };
            var expectedResponse = new ServiceResponse<GetPersonDto>
            {
                Message = "Validation error. Person was not updated."
            };

            _personServiceMock.Setup(p => p.UpdatePerson(updatedPersonDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().UpdatePerson(personID, updatedPersonDto);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task DeletePerson_ReturnsOk_WhenPersonIDIsValid()
        {
            //Arrange
            Guid personID = Guid.Parse("cfbe4568-6faf-4a3a-b7eb-6a73ce005bbc");
            var personToBeDeleted = examplePerson;
            personToBeDeleted.UserID = personID;
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Success = true,
                Data = new List<GetPersonDto>(),
                Message = $"Person with ID of '{personID}' was successfully deleted."
            };

            _personServiceMock.Setup(p => p.DeletePerson(personID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().DeletePerson(personID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, okResult.Value);
        }

        [Fact]
        public async Task DeletePerson_ReturnsNotFound_WhenPersonIDIsInvalid()
        {
            //Arrange
            Guid personID = Guid.NewGuid();
            var expectedResponse = new ServiceResponse<List<GetPersonDto>>
            {
                Message = $"Person with ID '{personID}' could not be found."
            };

            _personServiceMock.Setup(p => p.DeletePerson(personID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await PController().DeletePerson(personID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }
    }
}