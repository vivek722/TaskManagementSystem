using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Domain.interfaces;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.Repository;
using TaskManagementSystem.ResponseDto;
using TaskManagementSystem.services;
using Xunit;

namespace TaskManagemennt.Test.Service
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repoMock;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _repoMock = new Mock<ITaskRepository>();
            _service = new TaskService(_repoMock.Object);
        }

        [Fact]
        public async Task AddTask_DelegatesToRepository_AndReturnsResult()
        {
            var t = new TaskManage { Id = 1, Name = "x" };
            _repoMock.Setup(r => r.AddTask(t)).ReturnsAsync(t);

            var res = await _service.AddTask(t);

            Assert.Equal(1, res.Id);
            _repoMock.Verify(r => r.AddTask(t), Times.Once);
        }

        [Fact]
        public async Task GetByIdTask_ReturnsNull_WhenRepositoryReturnsNull()
        {
            _repoMock.Setup(r => r.GetByIdTask(10)).ReturnsAsync((TaskManage)null);

            var res = await _service.GetByIdTask(10);

            Assert.Null(res);
            _repoMock.Verify(r => r.GetByIdTask(10), Times.Once);
        }

        [Fact]
        public async Task TotalTaskbyProject_ReturnsCount_FromRepository()
        {
            _repoMock.Setup(r => r.TotalTaskbyProject(5)).ReturnsAsync(7);

            var count = await _service.TotalTaskbyProject(5);

            Assert.Equal(7, count);
        }

        [Fact]
        public async Task DeleteTask_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.DeleteTask(2)).ReturnsAsync(true);

            var res = await _service.DeleteTask(2);

            Assert.True(res);
            _repoMock.Verify(r => r.DeleteTask(2), Times.Once);
        }
    }
}