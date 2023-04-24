using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Controllers;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.School;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Implementations;
using SchoolProject.Services.Interfaces;

namespace SchoolProject.Tests.Controllers
{
    public class SchoolControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ISchoolService> _schoolServiceMock;
        private readonly School exampleSchool = new School
        {
            SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e"),
            SchoolName = "Sydney School"
        };

        public SchoolControllerTests() 
        {
            var schoolService = new SchoolService(_dataContextMock.Object, _mapperMock.Object);
            _schoolServiceMock = new Mock<ISchoolService>();
        }

        private SchoolController SController()
        {
            return new SchoolController(_schoolServiceMock.Object);
        }

        [Fact]
        public async Task GetSchools_ReturnsOk_WhenSchoolsExist()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<List<GetSchoolDto>>
            {
                Data = new List<GetSchoolDto>
                {
                    new GetSchoolDto
                    {
                        SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e"),
                        SchoolName = "Sydney School"
                    },
                    new GetSchoolDto
                    {
                        SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                        SchoolName = "Hollywood School"
                    }
                }
            };

            _schoolServiceMock.Setup(s => s.GetSchools())
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().GetSchools();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetSchoolDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetSchools_ReturnsNotFound_WhenSchoolsDoNotExist()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<List<GetSchoolDto>>
            {
                Success = false,
                Data = null,
                Message = "Could not find any school data..."
            };

            _schoolServiceMock.Setup(s => s.GetSchools())
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().GetSchools();

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetSchoolById_ReturnsOk_WhenSchoolExists()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e");
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Data = new GetSchoolDto
                {
                    SchoolID = schoolID,
                    SchoolName = "Sydney School"
                }
            };

            _schoolServiceMock.Setup(s => s.GetSchoolById(schoolID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().GetSchoolById(schoolID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetSchoolDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetSchoolById_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Arrange
            Guid schoolID = Guid.NewGuid();
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Success = false,
                Data = null,
                Message = $"Could not find a school with ID of '{schoolID}'."
            };

            _schoolServiceMock.Setup(s => s.GetSchoolById(schoolID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().GetSchoolById(schoolID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetSchoolByName_ReturnsOk_WhenSchoolExists()
        {
            //Arrange
            string schoolName = "Sydney School";
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Data = new GetSchoolDto
                {
                    SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e"),
                    SchoolName = schoolName
                }
            };

            _schoolServiceMock.Setup(s => s.GetSchoolByName(schoolName))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().GetSchoolByName(schoolName);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetSchoolDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetSchoolByName_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Arrange
            string schoolName = "School";
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Success = false,
                Data = null,
                Message = $"School with the name of '{schoolName}' could not be found."
            };

            _schoolServiceMock.Setup(s => s.GetSchoolByName(schoolName))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().GetSchoolByName(schoolName);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task AddSchool_ReturnsCreated_WhenValidInput()
        {
            //Arrange
            var addSchoolDto = new AddSchoolDto
            {
                SchoolName = "New School"
            };
            var expectedResponse = new ServiceResponse<List<GetSchoolDto>>
            {
                Data = new List<GetSchoolDto>
                {
                    new GetSchoolDto
                    {
                        SchoolID = Guid.NewGuid(),
                        SchoolName = addSchoolDto.SchoolName
                    }
                }
            };

            _schoolServiceMock.Setup(s => s.AddSchool(addSchoolDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().AddSchool(addSchoolDto);

            //Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetSchoolDto>>>(createdResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task UpdateSchool_ReturnsOk_WhenSchoolIDIsValid()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e");
            var updatedSchoolDto = new UpdateSchoolDto
            {
                SchoolID = schoolID,
                SchoolName = "Sydney School"
            };
            var expectedSchool = exampleSchool;
            exampleSchool.SchoolName = "Syd School";
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Data = new GetSchoolDto
                {
                    SchoolID = expectedSchool.SchoolID,
                    SchoolName = expectedSchool.SchoolName
                }
            };

            _schoolServiceMock.Setup(s => s.UpdateSchool(updatedSchoolDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().UpdateSchool(schoolID, updatedSchoolDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetSchoolDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task UpdateSchool_ReturnsBadRequest_WhenSchoolIDsDontMatch()
        {
            //Arrange
            Guid schoolID = Guid.NewGuid();
            var updatedSchoolDto = new UpdateSchoolDto
            {
                SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e"),
                SchoolName = "Sydney School"
            };
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Message = "Bad request. Please check that the IDs match."
            };

            _schoolServiceMock.Setup(s => s.UpdateSchool(updatedSchoolDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().UpdateSchool(schoolID, updatedSchoolDto);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, badRequestResult.Value);
        }


        [Fact]
        public async Task UpdateSchool_ReturnsNotFound_WhenSchoolIDIsInvalid()
        {
            //Arrange
            Guid schoolID = Guid.NewGuid();
            var updatedSchoolDto = new UpdateSchoolDto
            {
                SchoolID = schoolID,
                SchoolName = "Sydney School"
            };
            var expectedResponse = new ServiceResponse<GetSchoolDto>
            {
                Message = $"School with ID of '{schoolID}' could not be found."
            };

            _schoolServiceMock.Setup(s => s.UpdateSchool(updatedSchoolDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().UpdateSchool(schoolID, updatedSchoolDto);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteSchool_ReturnsOk_WhenSchoolIDIsValid()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e");
            var schoolToBeDeleted = exampleSchool;
            schoolToBeDeleted.SchoolID = schoolID;            
            var expectedResponse = new ServiceResponse<List<GetSchoolDto>>
            {
                Success = true,
                Data = new List<GetSchoolDto>(),
                Message = $"School with ID of '{schoolID}' was successfully deleted."
            };

            _schoolServiceMock.Setup(s => s.DeleteSchool(schoolID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().DeleteSchool(schoolID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, okResult.Value);
        }

        [Fact]
        public async Task DeleteSchool_ReturnsNotFound_WhenSchoolIDIsInvalid()
        {
            //Arrange
            Guid schoolID = Guid.NewGuid();
            var expectedResponse = new ServiceResponse<List<GetSchoolDto>>
            {
                Message = $"School with ID of '{schoolID}' could not be found."
            };

            _schoolServiceMock.Setup(s => s.DeleteSchool(schoolID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await SController().DeleteSchool(schoolID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }
    }
}