using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Model;
using TaskManagementSystem.Repository;
using Xunit;


namespace TaskManagemennt.Test.Repository
{
    public class SubTaskRepositoryIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly SubTaskRepository _repository;

        public SubTaskRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repository = new SubTaskRepository(_context);
        }

        [Fact]
        public async Task AddTask_PersistsAndReturnsTrue()
        {
            var task = new TaskManage { Name = "Parent", projectId = 1, status = true };
            _context.TaskManages.Add(task);
            await _context.SaveChangesAsync();

            var sub = new SubTaskManeg { Name = "Sub", TaskManageid = task.Id, assignToid = 0, status = true };
            var ok = await _repository.AddTask(sub);

            Assert.True(ok);
            Assert.NotEqual(0, sub.Id);

            var fromDb = await _context.SubTaskManegs.FindAsync(sub.Id);
            Assert.Equal("Sub", fromDb.Name);
            Assert.Equal(task.Id, fromDb.TaskManageid);
        }

        [Fact]
        public async Task UpdateTask_UpdatesFieldsAndReturnsTrue()
        {
            var task = new TaskManage { Name = "Parent2", projectId = 1, status = true };
            _context.TaskManages.Add(task);
            await _context.SaveChangesAsync();

            var sub = new SubTaskManeg { Name = "Old", Description = "d", TaskManageid = task.Id, status = true };
            _context.SubTaskManegs.Add(sub);
            await _context.SaveChangesAsync();

            var updated = new SubTaskManeg { Name = "New", Description = "updated" };

            var ok = await _repository.UpdateTask(sub.Id, updated);

            Assert.True(ok);

            var fromDb = await _context.SubTaskManegs.FindAsync(sub.Id);
            Assert.Equal("New", fromDb.Name);
            Assert.Equal("updated", fromDb.Description);
        }

        [Fact]
        public async Task DeleteTask_RemovesEntity_AndReturnsTrue()
        {
            var sub = new SubTaskManeg { Name = "ToDelete", TaskManageid = 0, status = true };
            _context.SubTaskManegs.Add(sub);
            await _context.SaveChangesAsync();

            var ok = await _repository.DeleteTask(sub.Id);

            Assert.True(ok);
            Assert.Null(await _context.SubTaskManegs.FindAsync(sub.Id));
        }

        [Fact]
        public async Task DeleteTask_ReturnsFalse_WhenNotFound()
        {
            var ok = await _repository.DeleteTask(9999);
            Assert.False(ok);
        }

        public void Dispose() => _context?.Dispose();
    }
}