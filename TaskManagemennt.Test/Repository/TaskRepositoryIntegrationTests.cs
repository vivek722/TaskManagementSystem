using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Repository;
using TaskManagementSystem.Model;
using TaskManagementSystem.Data;
using Xunit;

namespace TaskManagemennt.Test.Repository
{
    public class TaskRepositoryIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddTask_PersistsAndReturnsTask()
        {
            var t = new TaskManage { Name = "RepoTask", projectId = 1, status = true };

            var saved = await _repository.AddTask(t);

            Assert.NotEqual(0, saved.Id);
            var fromDb = await _context.TaskManages.FindAsync(saved.Id);
            Assert.Equal("RepoTask", fromDb.Name);
        }

        [Fact]
        public async Task GetByIdTask_ReturnsTask_WhenExists()
        {
            var t = new TaskManage { Name = "G1", projectId = 2, status = true };
            _context.TaskManages.Add(t);
            await _context.SaveChangesAsync();

            var res = await _repository.GetByIdTask(t.Id);

            Assert.NotNull(res);
            Assert.Equal("G1", res.Name);
        }

        [Fact]
        public async Task DeleteTask_RemovesEntity_AndReturnsTrue()
        {
            var t = new TaskManage { Name = "ToDelete", projectId = 3, status = true };
            _context.TaskManages.Add(t);
            await _context.SaveChangesAsync();

            var ok = await _repository.DeleteTask(t.Id);

            Assert.True(ok);
            Assert.Null(await _context.TaskManages.FindAsync(t.Id));
        }

        [Fact]
        public async Task TotalTaskbyProject_ReturnsCorrectCount()
        {
            _context.TaskManages.AddRange(
                new TaskManage { projectId = 100, status = true },
                new TaskManage { projectId = 100, status = true },
                new TaskManage { projectId = 101, status = true }
            );
            await _context.SaveChangesAsync();

            var count = await _repository.TotalTaskbyProject(100);

            Assert.Equal(2, count);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}