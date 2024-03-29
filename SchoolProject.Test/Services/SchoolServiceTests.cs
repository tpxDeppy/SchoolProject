﻿using AutoMapper;
using Moq.EntityFrameworkCore;
using SchoolProject.Data;
using SchoolProject.Models.DataTransferObjs.School;
using SchoolProject.Models.Entities;
using SchoolProject.Services.Implementations;

namespace SchoolProject.Tests.Services
{
    public class SchoolServiceTests
    {
        private readonly Mock<DataContext> _dataContextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly SchoolService _schoolService;

        public SchoolServiceTests()
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
                    SchoolID = Guid.Parse("fd619e90-2c3d-441c-8ca2-ba278e6ea24d"),
                    SchoolName = "Hollywood School"
                }
            };
            DataMockSetup(dbSchools);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { SchoolID = s.SchoolID, SchoolName = s.SchoolName });

            //Act
            var result = await _schoolService.GetSchools();

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbSchools.Count, result.Data.Count);
            Assert.Equal(dbSchools[0].SchoolID, result.Data[0].SchoolID);
            Assert.Equal(dbSchools[0].SchoolName, result.Data[0].SchoolName);
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
                new School { SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e") }
            };
            DataMockSetup(dbSchools);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { SchoolID = s.SchoolID });

            //Act
            var result = await _schoolService.GetSchoolById(dbSchools[0].SchoolID);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbSchools[0].SchoolID, result.Data.SchoolID);
        }

        [Fact]
        public async Task GetSchoolById_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Arrange
            var dbSchool = new School { SchoolID = Guid.NewGuid() };
            _dataContextMock.Setup(s => s.School);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { SchoolID = s.SchoolID });

            //Act
            var result = await _schoolService.GetSchoolById(dbSchool.SchoolID);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task GetSchoolByName_ReturnsOk_WhenSchoolExists()
        {
            //Arrange
            var dbSchools = new List<School>() { new School { SchoolName = "LA School" } };
            DataMockSetup(dbSchools);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { SchoolName = s.SchoolName });

            //Act
            var result = await _schoolService.GetSchoolByName(dbSchools[0].SchoolName);

            //Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dbSchools[0].SchoolName, result.Data.SchoolName);
        }

        [Fact]
        public async Task GetSchoolByName_ReturnsNotFound_WhenSchoolDoesNotExist()
        {
            //Arrange
            var dbSchool = new School { SchoolName = "A School" };
            _dataContextMock.Setup(s => s.School);
            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns<School>(s => new GetSchoolDto { SchoolName = s.SchoolName });

            //Act
            var result = await _schoolService.GetSchoolByName(dbSchool.SchoolName);

            //Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task AddSchool_ReturnsValidServiceResponseWithAddedSchool_WhenValidInput()
        {
            //Arrange
            var addSchoolDto = new AddSchoolDto
            {
                SchoolName = "New School"
            };

            var expectedSchool = new School
            {
                SchoolID = Guid.NewGuid(),
                SchoolName = "New School"
            };

            string expectedMessage = $"Successfully created a school with the name of '{expectedSchool.SchoolName}'.";
            
            _mapperMock.Setup(s => s.Map<School>(It.IsAny<AddSchoolDto>())).Returns(expectedSchool);

            _mapperMock.Setup(s => s.Map<GetSchoolDto>(expectedSchool))
                       .Returns(new GetSchoolDto
                       {
                           SchoolID = expectedSchool.SchoolID,
                           SchoolName = expectedSchool.SchoolName
                       });            

            _dataContextMock.Setup(context => context.School)
                            .ReturnsDbSet(new List<School> { expectedSchool }.AsQueryable());

            //Act
            var result = await _schoolService.AddSchool(addSchoolDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<List<GetSchoolDto>>>(result);            
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Equal(expectedMessage, result.Message);
            Assert.Equal(expectedSchool.SchoolID, result.Data[0].SchoolID);
            Assert.Equal(expectedSchool.SchoolName, result.Data[0].SchoolName);
        }

        [Fact]
        public async Task UpdateSchool_ReturnsValidServiceResponseWithUpdatedSchool_WhenValidInput()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5");
            var updatedSchoolDto = new UpdateSchoolDto
            {
                SchoolID = schoolID,
                SchoolName = "LA School"
            };

            var expectedSchool = new School
            {
                SchoolID = schoolID,
                SchoolName = "Los Angeles School"
            };

            _mapperMock.Setup(s => s.Map<School>(updatedSchoolDto)).Returns(expectedSchool);

            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns(new GetSchoolDto
                       {
                           SchoolID = expectedSchool.SchoolID,
                           SchoolName = expectedSchool.SchoolName
                       });

            _dataContextMock.Setup(context => context.School)
                            .ReturnsDbSet(new List<School> { expectedSchool }.AsQueryable());

            //Act
            var result = await _schoolService.UpdateSchool(updatedSchoolDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceResponse<GetSchoolDto>>(result);
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Equal("Successfully updated.", result.Message);
            Assert.Equal(expectedSchool.SchoolID, result.Data.SchoolID);
            Assert.Equal(expectedSchool.SchoolName, result.Data.SchoolName);
        }

        [Fact]
        public async Task UpdateSchool_ReturnsCorrectServiceResponse_WhenInputIsInvalid()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fec6caef-ccf0-408f-b3e6-21c3c75e18c5");
            var updatedSchoolDto = new UpdateSchoolDto
            {
                SchoolID = Guid.NewGuid(),
                SchoolName = "LA School"
            };

            var expectedSchool = new School
            {
                SchoolID = schoolID,
                SchoolName = "Los Angeles School"
            };

            string expectedMessage = $"School with ID of '{updatedSchoolDto.SchoolID}' could not be found.";

            _mapperMock.Setup(s => s.Map<School>(updatedSchoolDto)).Returns(expectedSchool);

            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns(new GetSchoolDto
                       {
                           SchoolID = expectedSchool.SchoolID,
                           SchoolName = expectedSchool.SchoolName
                       });

            _dataContextMock.Setup(context => context.School)
                            .ReturnsDbSet(new List<School> { expectedSchool }.AsQueryable());

            //Act
            var result = await _schoolService.UpdateSchool(updatedSchoolDto);

            //Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public async Task DeleteSchool_ReturnsValidServiceResponse_WhenValidInput()
        {
            //Arrange
            Guid schoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e");
            var dbSchools = new List<School>()
            {
                new School { SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e") }
            };

            DataMockSetup(dbSchools);
            _dataContextMock.Setup(s => s.School.FindAsync(schoolID))
                            .ReturnsAsync(dbSchools[0]);

            _mapperMock.Setup(s => s.Map<GetSchoolDto>(It.IsAny<School>()))
                       .Returns(new GetSchoolDto
                       {
                           SchoolID = schoolID
                       });

            //Act
            var result = await _schoolService.DeleteSchool(schoolID);

            //Assert
            Assert.IsType<ServiceResponse<List<GetSchoolDto>>>(result);
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data[0]);
            Assert.True(result.Success);
            Assert.Empty(result.Message);
        }

        [Fact]
        public async Task DeleteSchool_ReturnsNotFoundMessage_WhenInputIsInvalid()
        {
            //Arrange
            Guid schoolID = Guid.NewGuid();
            var dbSchools = new List<School>()
            {
                new School { SchoolID = Guid.Parse("fc711e2f-de88-4537-8582-3f4ab10bb21e") }
            };

            string expectedMessage = $"School with ID of '{schoolID}' could not be found.";

            DataMockSetup(dbSchools);
            _dataContextMock.Setup(s => s.School.FindAsync(schoolID))
                            .ReturnsAsync(dbSchools[0]);

            //Act
            var result = await _schoolService.DeleteSchool(schoolID);

            //Assert            
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal(expectedMessage, result.Message);
        }
    }
}
