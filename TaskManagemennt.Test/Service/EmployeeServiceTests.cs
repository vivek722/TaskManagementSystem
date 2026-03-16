using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManagementSystem.Model;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.services;
using Xunit;

namespace TaskManagemennt.Test.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRePository> _repoMock;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _repoMock = new Mock<IEmployeeRePository>();
            _service = new EmployeeService(_repoMock.Object);
        }

        [Fact]
        public async Task AddEmployee_DelegatesToRepository_ReturnsTrue()
        {
            var emp = new EmployeeModel { Name = "E1", Email = "e@x.com" };
            _repoMock.Setup(r => r.AddEmployee(emp)).ReturnsAsync(true);

            var result = await _service.AddEmployee(emp);

            Assert.True(result);
            _repoMock.Verify(r => r.AddEmployee(emp), Times.Once);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsListFromRepository()
        {
            var list = new List<EmployeeModel> { new EmployeeModel { Name = "A" } };
            _repoMock.Setup(r => r.GetAllEmployees()).ReturnsAsync(list);

            var res = await _service.GetAllEmployees();

            Assert.Same(list, res);
        }

        [Fact]
        public async Task GetByIdEmployee_DelegatesToRepository()
        {
            var emp = new EmployeeModel { Id = 2, Name = "X" };
            _repoMock.Setup(r => r.GetByIdEmployee(2)).ReturnsAsync(emp);

            var res = await _service.GetByIdEmployee(2);

            Assert.Equal(2, res.Id);
            _repoMock.Verify(r => r.GetByIdEmployee(2), Times.Once);
        }

        [Fact]
        public async Task GetByEmilEmployee_DelegatesToRepository()
        {
            var emp = new EmployeeModel { Id = 3, Email = "e@x.com" };
            _repoMock.Setup(r => r.GetByEmilEmployee("e@x.com")).ReturnsAsync(emp);

            var res = await _service.GetByEmilEmployee("e@x.com");
            Assert.Equal("e@x.com", res.Email);
        }

        [Fact]
        public async Task DeleteEmployee_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.DeleteEmployee(5)).ReturnsAsync(true);

            var res = await _service.DeleteEmployee(5);

            Assert.True(res);
            _repoMock.Verify(r => r.DeleteEmployee(5), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsTrue_WhenRepositorySucceeds()
        {
            var emp = new EmployeeModel { Name = "U" };
            _repoMock.Setup(r => r.UpdateEmployee(1, emp)).ReturnsAsync(true);

            var res = await _service.UpdateEmployee(1, emp);

            Assert.True(res);
            _repoMock.Verify(r => r.UpdateEmployee(1, emp), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeWhichHaveNotanyTask_ReturnsList()
        {
            var list = new List<EmployeeModel> { new EmployeeModel { Id = 1, Name = "N" } };
            _repoMock.Setup(r => r.GetEmployeeWhichHaveNotanyTask()).ReturnsAsync(list);

            var res = await _service.GetEmployeeWhichHaveNotanyTask();

            Assert.Single(res);
        }

        [Fact]
        public async Task GetAllEmployeesWithProjectAndTask_ReturnsList()
        {
            var list = new List<EmployeeModel> { new EmployeeModel { Name = "P" } };
            _repoMock.Setup(r => r.GetAllEmployeesWithProjectAndTask()).ReturnsAsync(list);

            var res = await _service.GetAllEmployeesWithProjectAndTask();

            Assert.Single(res);
        }

        [Fact]
        public async Task GetByIdEmployeeWithProjectAndTask_ReturnsModel()
        {
            var emp = new EmployeeModel { Id = 99 };
            _repoMock.Setup(r => r.GetByIdEmployeeWithProjectAndTask(99)).ReturnsAsync(emp);

            var res = await _service.GetByIdEmployeeWithProjectAndTask(99);

            Assert.Equal(99, res.Id);
        }

        [Fact]
        public async Task EmployeeWithMostCompletedTask_ThrowsNotImplemented()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => _service.EmployeeWithMostCompletedTask(1));
        }

    }
}