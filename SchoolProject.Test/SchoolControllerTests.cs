using AutoMapper;
using Moq.EntityFrameworkCore;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.School;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Implementations;

namespace SchoolProject.Tests
{
    public class SchoolControllerTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly SchoolService _schoolService;

        public SchoolControllerTests()
        {
            _schoolService = new SchoolService(_dataContextMock.Object, _mapperMock.Object);
        }

        private void DataMockSetup(List<School> schools)
        {
            _dataContextMock.Setup(c => c.School).ReturnsDbSet(schools);
        }

        [Fact]
        public async Task GetSchools_ReturnsOk_WhenSchoolsExist()
        {
            //Arrange
            var dbSchools = new List<School>()
            {
                new School
                {
                    School_ID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                    School_name = "Hollywood School"
                } 
            };
            DataMockSetup(dbSchools);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { School_ID = s.School_ID, School_name = s.School_name });

            //Act
            var result = await _schoolService.GetSchools();

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbSchools.Count, result.Data.Count);
            Assert.Equal(dbSchools[0].School_ID, result.Data[0].School_ID);
            Assert.Equal(dbSchools[0].School_name, result.Data[0].School_name);
        }

        [Fact]
        public async Task GetSchools_ReturnsNotFound_WhenThereAreNoSchools()
        {
            //Arrange
            var dbSchools = new List<School>();
            DataMockSetup(dbSchools);

            //Act
            var result = await _schoolService.GetSchools();

            //Assert
            Assert.False(result.Success);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetSchoolById_ReturnsOk_WhenSchoolExists()
        {
            //Arrange
            var dbSchools = new List<School>()
            {
                new School { School_ID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e") } 
            };
            DataMockSetup(dbSchools);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { School_ID = s.School_ID });

            //Act
            var result = await _schoolService.GetSchoolById(dbSchools[0].School_ID);
            
            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbSchools[0].School_ID, result.Data.School_ID);
        }

        [Fact]
        public async Task GetSchoolById_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Arrange
            var dbSchool = new School { School_ID = Guid.NewGuid() };
            _dataContextMock.Setup(s => s.School);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { School_ID = s.School_ID });

            //Act
            var result = await _schoolService.GetSchoolById(dbSchool.School_ID);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetSchoolByName_ReturnsOk_WhenSchoolExists()
        {
            //Arrange
            var dbSchools = new List<School>() { new School { School_name = "LA School" } };
            DataMockSetup(dbSchools);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { School_name = s.School_name });

            //Act
            var result = await _schoolService.GetSchoolByName(dbSchools[0].School_name);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbSchools[0].School_name, result.Data.School_name);
        }

        [Fact]
        public async Task GetSchoolByName_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Arrange
            var dbSchool = new School { School_name = "A School" };
            _dataContextMock.Setup(s => s.School);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { School_name = s.School_name });

            //Act
            var result = await _schoolService.GetSchoolByName(dbSchool.School_name);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task AddSchool_ReturnsValidServiceResponseWithAddedSchool_WhenValidInput()
        {
            // Arrange
            var addSchoolDto = new AddSchoolDto
            {
                School_name = "New School"
            };

            var expectedSchool = new School
            {
                School_ID = Guid.NewGuid(),
                School_name = "New School"
            };

            var expectedResponse = new ServiceResponse<List<GetSchoolDto>>
            {
                Success = true,
                Message = $"Successfully created a school with the name of '{expectedSchool.School_name}'.",
                Data = new List<GetSchoolDto>
                {
                    new GetSchoolDto
                    {
                        School_ID = Guid.NewGuid(),
                        School_name = "New School"
                    }
                }
            };

            _mapperMock.Setup(s => s.Map<School>(addSchoolDto)).Returns(expectedSchool);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(expectedSchool)).Returns(expectedResponse.Data[0]);
            _dataContextMock.Setup(s => s.School.Add(expectedSchool));
            _dataContextMock.Setup(s => s.SaveChangesAsync(default)).ReturnsAsync(1);

            _dataContextMock.Setup(context => context.School)
                            .ReturnsDbSet(new List<School> { expectedSchool }.AsQueryable());

            // Act
            var result = await _schoolService.AddSchool(addSchoolDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetSchoolDto>>>(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Message, result.Message);
            Assert.NotNull(result.Data);
            Assert.Collection(result.Data, item =>
            {
                Assert.Equal(expectedResponse.Data[0].School_ID, item.School_ID);
                Assert.Equal(expectedResponse.Data[0].School_name, item.School_name);
            });
        }
    }
}
