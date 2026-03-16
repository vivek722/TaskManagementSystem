using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;
using TaskManagementSystem.DataResponseModel;
using TaskManagementSystem.interfaces;
using Xunit;

namespace TaskManagemennt.Test.Controllers
{
    public class ProjectControllerTests
    {
        private readonly Mock<IProjectServices> _projectServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProjectController _controller;

        public ProjectControllerTests()
        {
            _projectServiceMock = new Mock<IProjectServices>();
            _mapperMock = new Mock<IMapper>();

            _controller = new ProjectController(_projectServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetProjects_ReturnsOk_WhenProjectsExist()
        {
            // Arrange
            var projects = new List<projectModel> { new projectModel { Name = "P1" } };
            _projectServiceMock.Setup(s => s.GetAllProjects()).ReturnsAsync(projects);

            // Act
            var result = await _controller.GetProjects();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<projectModel>>>(ok.Value);
            Assert.Single(model.data);
        }

        [Fact]
        public async Task AddProjects_ReturnsOk_WhenDtoValid()
        {
            // Arrange
            var dto = new projectDto { Name = "New", ProjectManagerId = 42 };
            var mapped = new projectModel { Name = dto.Name };

            _mapperMock.Setup(m => m.Map<projectModel>(It.IsAny<projectDto>())).Returns(mapped);
            _projectServiceMock.Setup(s => s.AddProject(It.IsAny<projectModel>())).ReturnsAsync(true);

            // Act
            var result = await _controller.AddProjects(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<projectModel>>(ok.Value);
            Assert.Equal(dto.ProjectManagerId, model.data.employeeId);
            Assert.True(model.data.status);
        }

        [Fact]
        public async Task AddProjects_ReturnsNotFound_WhenDtoNull()
        {
            // Act
            var result = await _controller.AddProjects(null);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var err = Assert.IsType<TaskManagementSystem.DataResponseModel.DataErrorResponseModel>(notFound.Value);
            Assert.Equal("Project Data Is not valid", err.Message);
        }

        [Fact]
        public async Task GetAllPRojectWithTask_ReturnsOk_WhenServiceReturns()
        {
            // Arrange
            var list = new List<projectModel> { new projectModel { Name = "WithTasks" } };
            _projectServiceMock.Setup(s => s.GetAllProjectsWithTask()).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAllPRojectWithTask();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<projectModel>>>(ok.Value);
            Assert.Single(model.data);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            // Arrange
            _projectServiceMock.Setup(s => s.DeleteProject(7)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(7);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value);
        }

        [Fact]
        public async Task getSpecificProjectAllTask_ReturnsOk_WhenFound()
        {
            // Arrange
            var p = new projectModel { Id = 9, Name = "Specific" };
            _projectServiceMock.Setup(s => s.GetSpecificProjectsWithTask(9)).ReturnsAsync(p);

            // Act
            var result = await _controller.getSpecificProjectAllTask(9);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<projectModel>>(ok.Value);
            Assert.Equal(9, model.data.Id);
        }

        [Fact]
        public async Task GetProjectMemberdetails_ReturnsOk_WhenMembersFound()
        {
            // Arrange
            var members = new List<MemberDetails> { new MemberDetails { EmployeeName = "M1", Email = "m@x.com", role = RoleModel.Employee } };
            _projectServiceMock.Setup(s => s.MemberDetails(11)).ReturnsAsync(members);

            // Act
            var result = await _controller.GetProjectMemberdetails(11);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<MemberDetails>>>(ok.Value);
            Assert.Single(model.data);
        }

        [Fact]
        public async Task GetProjectWithOverDueTask_ReturnsOk_WhenFound()
        {
            // Arrange
            var p = new projectModel { Id = 15, Name = "OverDue" };
            _projectServiceMock.Setup(s => s.GetProjectWithOverDueTask(15)).ReturnsAsync(p);

            // Act
            var result = await _controller.GetProjectWithOverDueTask(15);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<projectModel>>(ok.Value);
            Assert.Equal(15, model.data.Id);
        }

        [Fact]
        public async Task UpdateProject_ReturnsOk_WhenDtoValid()
        {
            // Arrange
            var dto = new projectDto { Name = "U", ProjectManagerId = 2 };
            var mapped = new projectModel { Name = dto.Name, CreatedAt = DateTime.UtcNow.AddDays(-2) };
            _mapperMock.Setup(m => m.Map<projectModel>(It.IsAny<projectDto>())).Returns(mapped);
            _projectServiceMock.Setup(s => s.UpdateProject(5, It.IsAny<projectModel>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProject(5, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<projectModel>>(ok.Value);
            Assert.Equal("UpdateProject successfully", model.Message);
        }
    }
}