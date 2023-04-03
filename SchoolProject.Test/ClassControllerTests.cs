using AutoMapper;
using Moq.EntityFrameworkCore;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.Class;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Implementations;

namespace SchoolProject.Tests
{
    public class ClassControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ClassService _classService;

        public ClassControllerTests()
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
                    Class_ID = Guid.Parse("ebb7a0d9-730f-40f1-9b9c-541f371074ba"),
                    Class_name = "Scripting"
                }
            };
            DataMockSetup(dbClasses);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { Class_ID = c.Class_ID, Class_name = c.Class_name });

            //Act
            var result = await _classService.GetAllClasses();

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbClasses.Count, result.Data.Count);
            Assert.Equal(dbClasses[0].Class_ID, result.Data[0].Class_ID);
            Assert.Equal(dbClasses[0].Class_name, result.Data[0].Class_name);
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
                new Class { Class_ID = Guid.Parse("b7f068af-3856-4d1b-9023-91a3d01ac1e0") } 
            };
            DataMockSetup(dbClasses);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { Class_ID = c.Class_ID });

            //Act
            var result = await _classService.GetClassById(dbClasses[0].Class_ID);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbClasses[0].Class_ID, result.Data.Class_ID);
        }

        [Fact]
        public async Task GetClassById_ReturnsNotFound_WhenClassDoesNotExist()
        {
            //Arrange
            var dbClass = new Class { Class_ID = Guid.NewGuid() };
            _dataContextMock.Setup(c => c.Class);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { Class_ID = c.Class_ID });

            //Act
            var result = await _classService.GetClassById(dbClass.Class_ID);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetClassByName_ReturnsOk_WhenClassExists()
        {
            //Arrange
            var dbClasses = new List<Class>() { new Class { Class_name = "Singing" } };
            DataMockSetup(dbClasses);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { Class_name = c.Class_name });

            //Act
            var result = await _classService.GetClassByName(dbClasses[0].Class_name);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbClasses[0].Class_name, result.Data.Class_name);
        }

        [Fact]
        public async Task GetClassByName_ReturnsNotFound_WhenClassDoesNotExist()
        {
            //Arrange
            var dbClass = new Class { Class_name = "Math" };
            _dataContextMock.Setup(c => c.Class);
            _mapperMock.Setup(c => c.Map<GetClassDto>(It.IsAny<Class>()))
                       .Returns<Class>(c => new GetClassDto { Class_name = c.Class_name });

            //Act
            var result = await _classService.GetClassByName(dbClass.Class_name);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task AddClass_ReturnsValidServiceResponseWithAddedClass_WhenValidInput()
        {
            // Arrange
            var addClassDto = new AddClassDto
            {
                Class_name = "New Class",
                Class_description = "New Class description"
            };

            var expectedClass = new Class
            {
                Class_ID = Guid.NewGuid(),
                Class_name = "New Class",
                Class_description = "New Class description"
            };

            var expectedResponse = new ServiceResponse<List<GetClassDto>>
            {
                Success = true,
                Message = $"Successfully created a class with the name of '{expectedClass.Class_name}'.",
                Data = new List<GetClassDto>
                {
                    new GetClassDto
                    {
                        Class_ID = Guid.NewGuid(),
                        Class_name = "New Class",
                        Class_description = "New Class description"
                    }
                }
            };

            _mapperMock.Setup(c => c.Map<Class>(addClassDto)).Returns(expectedClass);
            _mapperMock.Setup(c => c.Map<GetClassDto>(expectedClass)).Returns(expectedResponse.Data[0]);
            _dataContextMock.Setup(c => c.Class.Add(expectedClass));
            _dataContextMock.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            _dataContextMock.Setup(context => context.Class)
                            .ReturnsDbSet(new List<Class> { expectedClass }.AsQueryable());

            // Act
            var result = await _classService.AddClass(addClassDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetClassDto>>>(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Message, result.Message);
            Assert.NotNull(result.Data);
            Assert.Collection(result.Data, item =>
            {
                Assert.Equal(expectedResponse.Data[0].Class_ID, item.Class_ID);
                Assert.Equal(expectedResponse.Data[0].Class_name, item.Class_name);
                Assert.Equal(expectedResponse.Data[0].Class_description, item.Class_description);
            });
        }
    }
}
