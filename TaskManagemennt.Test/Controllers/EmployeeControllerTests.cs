using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Domain.interfaces;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.Model;
using TaskManagementSystem.Utility.CacheService;
using TaskManagementSystem.DataResponseModel;
using TaskManagementSystem.interfaces;
using Xunit;

namespace TaskManagemennt.Test.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeServices> _employeeServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAiService> _aiServiceMock;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeServices>();
            _cacheServiceMock = new Mock<ICacheService>();
            _mapperMock = new Mock<IMapper>();
            _aiServiceMock = new Mock<IAiService>();

            _controller = new EmployeeController(
                _employeeServiceMock.Object,
                _cacheServiceMock.Object,
                _mapperMock.Object,
                _aiServiceMock.Object
            );
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsCached_WhenCacheHasData()
        {
            // Arrange
            var cached = new List<EmployeeDto> { new EmployeeDto { Name = "Alice", Email = "a@x.com", password = "p" } };
            _cacheServiceMock.Setup(c => c.GetAsync<List<EmployeeDto>>(It.Is<string>(k => k == "EmployeesCache")))
                .ReturnsAsync(cached);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<EmployeeDto>>>(ok.Value);
            Assert.Single(model.data);
            _employeeServiceMock.Verify(s => s.GetAllEmployees(), Times.Never);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsFromServiceAndSetsCache_WhenCacheEmpty()
        {
            // Arrange
            _cacheServiceMock.Setup(c => c.GetAsync<List<EmployeeDto>>(It.IsAny<string>()))
                .ReturnsAsync((List<EmployeeDto>)null);

            var employees = new List<EmployeeModel> { new EmployeeModel { Name = "Bob", Email = "b@x.com" } };
            _employeeServiceMock.Setup(s => s.GetAllEmployees()).ReturnsAsync(employees);
            _cacheServiceMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<List<EmployeeModel>>(), It.IsAny<TimeSpan?>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<EmployeeModel>>>(ok.Value);
            Assert.Single(model.data);
            _cacheServiceMock.Verify(c => c.SetAsync("EmployeesCache", employees, null), Times.Once);
        }

        [Fact]
        public async Task AddEmployees_ReturnsOk_WithHashedPassword()
        {
            // Arrange
            var dto = new EmployeeDto { Name = "Sam", Email = "s@x.com", password = "plainpass" };
            var mapped = new EmployeeModel { Name = dto.Name, Email = dto.Email, password = dto.password };

            _mapperMock.Setup(m => m.Map<EmployeeModel>(It.IsAny<EmployeeDto>())).Returns(mapped);
            _employeeServiceMock.Setup(s => s.AddEmployee(It.IsAny<EmployeeModel>())).ReturnsAsync(true);

            // Act
            var result = await _controller.AddEmployees(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<EmployeeModel>>(ok.Value);
            Assert.Equal(dto.Name, model.data.Name);
            Assert.NotEqual(dto.password, model.data.password);
            Assert.True(BCrypt.Net.BCrypt.Verify(dto.password, model.data.password));
        }

        [Fact]
        public async Task AddEmployees_ReturnsNotFound_WhenDtoIsNull()
        {
            // Act
            var result = await _controller.AddEmployees(null);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var err = Assert.IsType<TaskManagementSystem.DataResponseModel.DataErrorResponseModel>(notFound.Value);
            Assert.Equal("invalid Employee Data", err.Message);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsOk_WhenServiceDeletes()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.DeleteEmployee(5)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEmployee(5);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)ok.Value);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.DeleteEmployee(6)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteEmployee(6);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var err = Assert.IsType<TaskManagementSystem.DataResponseModel.DataErrorResponseModel>(notFound.Value);
            Assert.Equal("Employee not found", err.Message);
        }

        [Fact]
        public async Task DeleteEmployee_Returns500_OnException()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.DeleteEmployee(It.IsAny<int>())).ThrowsAsync(new Exception("boom"));

            // Act
            var result = await _controller.DeleteEmployee(1);

            // Assert
            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
            Assert.Equal("boom", obj.Value);
        }

        [Fact]
        public async Task AiChatbot_ReturnsBadRequest_WhenInputEmpty()
        {
            // Act
            var result = await _controller.AiChatbot(string.Empty);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var err = Assert.IsType<TaskManagementSystem.DataResponseModel.DataErrorResponseModel>(bad.Value);
            Assert.Equal("User input cannot be empty", err.Message);
        }

        [Fact]
        public async Task AiChatbot_ReturnsOk_WithAiResponse()
        {
            // Arrange
            _aiServiceMock.Setup(a => a.Chat("hello")).ReturnsAsync("reply");

            // Act
            var result = await _controller.AiChatbot("hello");

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<string>>(ok.Value);
            Assert.Equal("reply", model.data);
        }

        [Fact]
        public async Task UpdateEmployees_ReturnsOk_WhenDtoProvided()
        {
            // Arrange
            var dto = new EmployeeDto { Name = "U", Email = "u@x.com" };
            var mapped = new EmployeeModel { Name = dto.Name, Email = dto.Email, CreatedAt = DateTime.UtcNow.AddDays(-1) };

            _mapperMock.Setup(m => m.Map<EmployeeModel>(It.IsAny<EmployeeDto>())).Returns(mapped);
            _employeeServiceMock.Setup(s => s.UpdateEmployee(10, It.IsAny<EmployeeModel>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateEmployees(10, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<EmployeeModel>>(ok.Value);
            Assert.Equal("UpdateEmployees successfully", model.Message);
        }

        [Fact]
        public async Task GetEmployeesWithProjectAndTask_ReturnsOk_WhenServiceReturnsData()
        {
            // Arrange
            var list = new List<EmployeeModel> { new EmployeeModel { Name = "E1" } };
            _employeeServiceMock.Setup(s => s.GetAllEmployeesWithProjectAndTask()).ReturnsAsync(list);

            // Act
            var result = await _controller.GetEmployeesWithProjectAndTask();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<List<EmployeeModel>>>(ok.Value);
            Assert.Single(model.data);
        }

        [Fact]
        public async Task GetSpecificemployeesWithProjectAndTask_ReturnsOk_WhenFound()
        {
            // Arrange
            var emp = new EmployeeModel { Id = 3, Name = "John" };
            _employeeServiceMock.Setup(s => s.GetByIdEmployeeWithProjectAndTask(3)).ReturnsAsync(emp);

            // Act
            var result = await _controller.GetSpecificemployeesWithProjectAndTask(3);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<DataSuccessResponseModel<EmployeeModel>>(ok.Value);
            Assert.Equal(3, model.data.Id);
        }
    }
}