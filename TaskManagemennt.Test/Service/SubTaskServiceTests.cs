using Moq;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.services;
using Xunit;

namespace TaskManagemennt.Test.Service
{
    public class SubTaskServiceTests
    {
        private readonly Mock<ISubTaskRepository> _repoMock;
        private readonly SubTaskService _service;

        public SubTaskServiceTests()
        {
            _repoMock = new Mock<ISubTaskRepository>();
            _service = new SubTaskService(_repoMock.Object);
        }

        [Fact]
        public async Task AddTask_DelegatesToRepository_ReturnsTrue()
        {
            var sub = new SubTaskManeg { Name = "S1" };
            _repoMock.Setup(r => r.AddTask(sub)).ReturnsAsync(true);

            var res = await _service.AddTask(sub);

            Assert.True(res);
            _repoMock.Verify(r => r.AddTask(sub), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_DelegatesToRepository_ReturnsTrue()
        {
            _repoMock.Setup(r => r.DeleteTask(10)).ReturnsAsync(true);

            var res = await _service.DeleteTask(10);

            Assert.True(res);
            _repoMock.Verify(r => r.DeleteTask(10), Times.Once);
        }

        [Fact]
        public async Task UpdateTask_DelegatesToRepository_ReturnsTrue()
        {
            var sub = new SubTaskManeg { Id = 5, Name = "Up" };
            _repoMock.Setup(r => r.UpdateTask(5, sub)).ReturnsAsync(true);

            var res = await _service.UpdateTask(5, sub);

            Assert.True(res);
            _repoMock.Verify(r => r.UpdateTask(5, sub), Times.Once);
        }

        [Fact]
        public async Task UpdateTask_ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            var sub = new SubTaskManeg { Id = 99, Name = "Not" };
            _repoMock.Setup(r => r.UpdateTask(99, sub)).ReturnsAsync(false);

            var res = await _service.UpdateTask(99, sub);

            Assert.False(res);
        }
    }
}