using AutoMapper;
using Moq.EntityFrameworkCore;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Implementations;

namespace SchoolProject.Tests.Services
{
    public class ClassServiceTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ClassService _classService;

        public ClassServiceTests()
        {
            _classService = new ClassService(_dataContextMock.Object, _mapperMock.Object);
        }

        private void DataMockSetup(List<Class> classes)
        {
            _dataContextMock.Setup(c => c.Class).ReturnsDbSet(classes);
        }

        [Fact]
        public async Task GetAllClasses_ReturnsOk_WhenClassesExist()
        {
            //Arrange
            var dbClasses = new List<Class>()
            {
                new Class
                {
                    ClassID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                    ClassName = "Scripting"
                }
            };
            DataMockSetup(dbClasses);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { ClassID = c.ClassID, ClassName = c.ClassName });

            //Act
            var result = await _classService.GetAllClasses();

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbClasses.Count, result.Data.Count);
            Assert.Equal(dbClasses[0].ClassID, result.Data[0].ClassID);
            Assert.Equal(dbClasses[0].ClassName, result.Data[0].ClassName);
        }

        [Fact]
        public async Task GetAllClasses_ReturnsNotFound_WhenThereAreNoClasses()
        {
            //Arrange
            var dbClasses = new List<Class>();
            DataMockSetup(dbClasses);

            //Act
            var result = await _classService.GetAllClasses();

            //Assert
            Assert.False(result.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetClassById_ReturnsOk_WhenClassExists()
        {
            //Arrange
            var dbClasses = new List<Class>()
            {
                new Class { ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0") }
            };
            DataMockSetup(dbClasses);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { ClassID = c.ClassID });

            //Act
            var result = await _classService.GetClassById(dbClasses[0].ClassID);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbClasses[0].ClassID, result.Data.ClassID);
        }

        [Fact]
        public async Task GetClassById_ReturnsNotFound_WhenClassDoesNotExist()
        {
            //Arrange
            var dbClass = new Class { ClassID = Guid.NewGuid() };
            _dataContextMock.Setup(c => c.Class);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { ClassID = c.ClassID });

            //Act
            var result = await _classService.GetClassById(dbClass.ClassID);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetClassByName_ReturnsOk_WhenClassExists()
        {
            //Arrange
            var dbClasses = new List<Class>() { new Class { ClassName = "Singing" } };
            DataMockSetup(dbClasses);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { ClassName = c.ClassName });

            //Act
            var result = await _classService.GetClassByName(dbClasses[0].ClassName);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbClasses[0].ClassName, result.Data.ClassName);
        }

        [Fact]
        public async Task GetClassByName_ReturnsNotFound_WhenClassDoesNotExist()
        {
            //Arrange
            var dbClass = new Class { ClassName = "Math" };
            _dataContextMock.Setup(c => c.Class);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { ClassName = c.ClassName });

            //Act
            var result = await _classService.GetClassByName(dbClass.ClassName);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task AddClass_ReturnsValidServiceResponseWithAddedClass_WhenValidInput()
        {
            //Arrange
            var addClassDto = new AddClassDto
            {
                ClassName = "New Class",
                ClassDescription = "New Class description"
            };

            var expectedClass = new Class
            {
                ClassID = Guid.NewGuid(),
                ClassName = "New Class",
                ClassDescription = "New Class description"
            };
            string expectedMessage = $"Successfully created a class with the name of '{expectedClass.ClassName}'.";

            _mapperMock.Setup(c => c.Map<Class>(It.IsAny<AddClassDto>())).Returns(expectedClass);
            
            _mapperMock.Setup(c => c.Map<GetClassDto>(expectedClass))
                       .Returns(new GetClassDto
                       {
                           ClassID = expectedClass.ClassID,
                           ClassName = expectedClass.ClassName,
                           ClassDescription = expectedClass.ClassDescription
                       });

            _dataContextMock.Setup(context => context.Class)
                            .ReturnsDbSet(new List<Class> { expectedClass }.AsQueryable());

            //Act
            var result = await _classService.AddClass(addClassDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetClassDto>>>(result);
            Assert.True(result.Success);
            Assert.Equal(expectedMessage, result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedClass.ClassID, result.Data[0].ClassID);
            Assert.Equal(expectedClass.ClassName, result.Data[0].ClassName);
            Assert.Equal(expectedClass.ClassDescription, result.Data[0].ClassDescription);
        }

        [Fact]
        public async Task UpdateClass_ReturnsValidServiceResponseWithUpdatedClass_WhenValidInput()
        {
            //Arrange
            var updatedClassDto = new UpdateClassDto
            {
                ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                ClassName = "Acting",
                ClassDescription = "How to act"
            };

            var expectedClass = new Class
            {
                ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                ClassName = "Acting",
                ClassDescription = "How to act - part 1"
            };

            _mapperMock.Setup(c => c.Map<Class>(updatedClassDto)).Returns(expectedClass);

            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns(new GetClassDto
                       {
                           ClassID = expectedClass.ClassID,
                           ClassName = expectedClass.ClassName,
                           ClassDescription = expectedClass.ClassDescription
                       });

            _dataContextMock.Setup(context => context.Class)
                            .ReturnsDbSet(new List<Class> { expectedClass }.AsQueryable());

            //Act
            var result = await _classService.UpdateClass(updatedClassDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<GetClassDto>>(result);
            Assert.True(result.Success);
            Assert.Equal("Successfully updated.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedClass.ClassID, result.Data.ClassID);
            Assert.Equal(expectedClass.ClassName, result.Data.ClassName);
            Assert.Equal(expectedClass.ClassDescription, result.Data.ClassDescription);
        }

        [Fact]
        public async Task UpdateClass_ReturnsNotFoundMessage_WhenInputIsInvalid()
        {
            //Arrange
            var updatedClassDto = new UpdateClassDto
            {
                ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0"),
                ClassName = "Acting",
                ClassDescription = "How to act"
            };

            var expectedClass = new Class
            {
                ClassID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e8"),
                ClassName = "Acting",
                ClassDescription = "How to act - part 1"
            };
            string expectedMessage = $"Class with ID of '{updatedClassDto.ClassID}' could not be found.";

            _mapperMock.Setup(c => c.Map<Class>(updatedClassDto)).Returns(expectedClass);

            _dataContextMock.Setup(context => context.Class)
                            .ReturnsDbSet(new List<Class> { expectedClass }.AsQueryable());

            //Act
            var result = await _classService.UpdateClass(updatedClassDto);

            //Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal(expectedMessage, result.Message);            
        }

        [Fact]
        public async Task DeleteClass_ReturnsValidServiceResponse_WhenValidInput()
        {
            //Arrange
            Guid classID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba");
            var dbClasses = new List<Class>()
            {
                new Class
                {
                    ClassID = classID,
                    ClassName = "Scripting",
                    ClassDescription = "How to write movie scripts"
                }
            };

            DataMockSetup(dbClasses);
            _dataContextMock.Setup(c => c.Class.FindAsync(classID))
                            .ReturnsAsync(dbClasses[0]);

            _mapperMock.Setup(c => c.Map<GetClassDto>(dbClasses[0]))
                       .Returns(new GetClassDto
                       {
                           ClassID = dbClasses[0].ClassID,
                           ClassName = dbClasses[0].ClassName,
                           ClassDescription = dbClasses[0].ClassDescription
                       });

            //Act
            var result = await _classService.DeleteClass(classID);

            //Assert
            Assert.IsType<ServiceResponse<List<GetClassDto>>>(result);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data[0]);
            Assert.True(result.Success);
            Assert.Empty(result.Message);
        }

        [Fact]
        public async Task DeleteClass_ReturnsNotFoundMessage_WhenInputIsInvalid()
        {
            //Arrange
            Guid classID = Guid.NewGuid();
            var dbClasses = new List<Class>()
            {
                new Class
                {
                    ClassID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                    ClassName = "Scripting",
                    ClassDescription = "How to write movie scripts"
                }
            };

            DataMockSetup(dbClasses);
            _dataContextMock.Setup(c => c.Class.FindAsync(classID))
                            .ReturnsAsync(dbClasses[0]);

            //Act
            var result = await _classService.DeleteClass(classID);

            //Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal($"Class with ID of '{classID}' could not be found.", result.Message);
        }
    }
}
