using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.API.Controllers;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Implementations;
using SchoolProject.Services.Interfaces;

namespace SchoolProject.Tests.Controllers
{
    public class ClassControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IClassService> _classServiceMock;

        public ClassControllerTests()
        {
            var classService = new ClassService(_dataContextMock.Object, _mapperMock.Object);
            _classServiceMock = new Mock<IClassService>();
        }

        private ClassController CController()
        {
            return new ClassController(_classServiceMock.Object);
        }

        [Fact]
        public async Task GetAllClasses_ReturnsOk_WhenClassesExist()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<List<GetClassDto>>
            {
                Data = new List<GetClassDto>
                {
                    new GetClassDto
                    {
                        Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                        Class_name = "Scripting",
                        Class_description = "How to write movie scripts"
                    },
                    new GetClassDto
                    {
                        Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                        Class_name = "Acting",
                        Class_description = "How to act"
                    }
                }
            };

            _classServiceMock.Setup(c => c.GetAllClasses())
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().GetAllClasses();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetClassDto>>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetAllClasses_ReturnsNotFound_WhenClassesDoNotExist()
        {
            //Arrange
            var expectedResponse = new ServiceResponse<List<GetClassDto>>
            {
                Success = false,
                Data = null,
                Message = "Could not find any data..."
            };

            _classServiceMock.Setup(c => c.GetAllClasses())
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().GetAllClasses();

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetClasseById_ReturnsOk_WhenClassExists()
        {
            //Arrange
            Guid classID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba");
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Data = new GetClassDto
                {
                    Class_ID = classID,
                    Class_name = "Scripting",
                    Class_description = "How to write movie scripts"                
                }
            };

            _classServiceMock.Setup(c => c.GetClassById(classID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().GetClassById(classID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetClassDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetClasseById_ReturnsNotFound_WhenClassDoesNotExist()
        {
            //Arrange
            Guid classID = Guid.NewGuid();
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Data = null,
                Message = $"Class with ID of '{classID}' could not be found."
            };

            _classServiceMock.Setup(c => c.GetClassById(classID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().GetClassById(classID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task GetClassByName_ReturnsOk_WhenClassExists()
        {
            //Arrange
            string className = "Scripting";
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Data = new GetClassDto
                {
                    Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                    Class_name = className,
                    Class_description = "How to write movie scripts"
                }
            };

            _classServiceMock.Setup(c => c.GetClassByName(className))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().GetClassByName(className);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetClassDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task GetClassByName_ReturnsNotFound_WhenClassDoesNotExist()
        {
            //Arrange
            string className = "Script";
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Data = null,
                Message = $"Class with the name of '{className}' could not be found."
            };

            _classServiceMock.Setup(c => c.GetClassByName(className))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().GetClassByName(className);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task AddClass_ReturnsCreated_WhenValidInput()
        {
            //Arrange
            var addClassDto = new AddClassDto
            {
                Class_name = "New Class",
                Class_description = "New Class Description",
            };            
            var expectedResponse = new ServiceResponse<List<GetClassDto>>
            {
                Data = new List<GetClassDto>
                {
                    new GetClassDto
                    {
                        Class_ID = Guid.NewGuid(),
                        Class_name = addClassDto.Class_name,
                        Class_description = addClassDto.Class_description
                    }                    
                }
            };

            _classServiceMock.Setup(c => c.AddClass(addClassDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().AddClass(addClassDto);

            //Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<List<GetClassDto>>>(createdResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task UpdateClass_ReturnsOk_WhenValidID()
        {
            //Arrange
            Guid classID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba");
            var updatedClassDto = new UpdateClassDto
            {
                Class_ID = classID,
                Class_name = "A Class",
                Class_description = "A Class Description",
            };
            var expectedClass = new Class
            {
                Class_ID = classID,
                Class_name = "Updated Class",
                Class_description = "An Updated Class Description"
            };
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Data = new GetClassDto
                {
                    Class_ID = expectedClass.Class_ID,
                    Class_name = expectedClass.Class_name,
                    Class_description = expectedClass.Class_description
                }
            };

            _classServiceMock.Setup(c => c.UpdateClass(updatedClassDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().UpdateClass(classID, updatedClassDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultValue = Assert.IsType<ServiceResponse<GetClassDto>>(okResult.Value);
            Assert.Equal(expectedResponse, resultValue);
        }

        [Fact]
        public async Task UpdateClass_ReturnsBadRequest_WhenClassIDsDontMatch()
        {
            //Arrange
            Guid classID = Guid.NewGuid();
            var updatedClassDto = new UpdateClassDto
            {
                Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                Class_name = "A Class",
                Class_description = "A Class Description",
            };
            var expectedClass = new Class
            {
                Class_ID = classID,
                Class_name = "Updated Class",
                Class_description = "An Updated Class Description"
            };
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Message = "Bad request. Please check that the IDs match."
            };

            _classServiceMock.Setup(c => c.UpdateClass(updatedClassDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().UpdateClass(classID, updatedClassDto);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateClass_ReturnsNotFound_WhenClassIDIsInvalid()
        {
            //Arrange
            Guid classID = Guid.NewGuid();
            var updatedClassDto = new UpdateClassDto
            {
                Class_ID = classID,
                Class_name = "A Class",
                Class_description = "A Class Description",
            };
            var expectedClass = new Class
            {
                Class_ID = classID,
                Class_name = "Updated Class",
                Class_description = "An Updated Class Description"
            };
            var expectedResponse = new ServiceResponse<GetClassDto>
            {
                Message = $"Class with ID of '{classID}' could not be found."
            };

            _classServiceMock.Setup(c => c.UpdateClass(updatedClassDto))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().UpdateClass(classID, updatedClassDto);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteClass_ReturnsOk_WhenClassIDIsValid()
        {
            //Arrange
            Guid classID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0");
            var classToBeDeleted = new Class
            {
                Class_ID = classID,
                Class_name = "Acting",
                Class_description = "How to act"
            };
            var expectedResponse = new ServiceResponse<List<GetClassDto>>
            {
                Success = true,
                Data = new List<GetClassDto>(),
                Message = $"Class with ID of '{classID}' was successfully deleted."
            };

            _classServiceMock.Setup(c => c.DeleteClass(classID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().DeleteClass(classID);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, okResult.Value);
        }

        [Fact]
        public async Task DeleteClass_ReturnsNotFound_WhenClassIDIsInvalid()
        {
            //Arrange
            Guid classID = Guid.NewGuid();
            var classToBeDeleted = new Class
            {
                Class_ID = classID,
            };
            var expectedResponse = new ServiceResponse<List<GetClassDto>>()
            {
                Message = $"Class with ID of '{classID}' could not be found."
            };

            _classServiceMock.Setup(c => c.DeleteClass(classID))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await CController().DeleteClass(classID);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedResponse.Message, notFoundResult.Value);            
        }
    }
}