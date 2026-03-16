using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Repository;
using TaskManagementSystem.Model;
using TaskManagementSystem.Data;
using TaskManagementSystem.ResponseDto;
using Xunit;

namespace TaskManagemennt.Test.Repository
{
    public class ProjectRepositoryIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ProjectRepository _repository;

        public ProjectRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new ProjectRepository(_context);
        }

        [Fact]
        public async Task AddProject_PersistsAndReturnsTrue()
        {
            var manager = new EmployeeModel { Name = "PM", Email = "pm@x.com", status = true };
            _context.Employees.Add(manager);
            await _context.SaveChangesAsync();

            var project = new projectModel { Name = "RepoProject", employeeId = manager.Id, status = true };
            var ok = await _repository.AddProject(project);

            Assert.True(ok);
            Assert.NotEqual(0, project.Id);

            var fromDb = await _context.Projects.FindAsync(project.Id);
            Assert.Equal("RepoProject", fromDb.Name);
        }

        [Fact]
        public async Task GetAllProjects_ReturnsOnlyActiveProjects()
        {
            _context.Projects.AddRange(
                new projectModel { Name = "P1", status = true },
                new projectModel { Name = "P2", status = false },
                new projectModel { Name = "P3", status = true }
            );
            await _context.SaveChangesAsync();

            var list = await _repository.GetAllProjects();
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetSpecificProjectsWithTask_ReturnsProjectWithTasks()
        {
            var manager = new EmployeeModel { Name = "M", Email = "m@x.com", status = true };
            _context.Employees.Add(manager);
            await _context.SaveChangesAsync();

            var project = new projectModel { Name = "PTasks", employeeId = manager.Id, status = true };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var task = new TaskManage { Name = "T", projectId = project.Id, assignToId = manager.Id, status = true };
            _context.TaskManages.Add(task);
            await _context.SaveChangesAsync();

            var res = await _repository.GetSpecificProjectsWithTask(project.Id);

            Assert.NotNull(res);
            Assert.NotNull(res.TaskManages);
            Assert.Single(res.TaskManages);
        }

        [Fact]
        public async Task GetProjectWithOverDueTask_ReturnsProject_WhenOverdueExists()
        {
            var manager = new EmployeeModel { Name = "M2", Email = "m2@x.com", status = true };
            _context.Employees.Add(manager);
            await _context.SaveChangesAsync();

            var project = new projectModel { Name = "OverDueProj", employeeId = manager.Id, status = true };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var overdueTask = new TaskManage
            {
                Name = "OverdueTask",
                projectId = project.Id,
                assignToId = manager.Id,
                dueDate = DateTime.UtcNow.AddDays(-2),
                taskStatus = status.Pending,
                status = true
            };
            _context.TaskManages.Add(overdueTask);
            await _context.SaveChangesAsync();

            var res = await _repository.GetProjectWithOverDueTask(project.Id);

            Assert.NotNull(res);
            Assert.NotNull(res.TaskManages);
            Assert.Single(res.TaskManages);
        }

        [Fact]
        public async Task MemberDetails_ReturnsDetails_ForProject()
        {
            var manager = new EmployeeModel { Name = "Member", Email = "mem@x.com", role = RoleModel.Employee, status = true };
            _context.Employees.Add(manager);
            await _context.SaveChangesAsync();

            var project = new projectModel { Name = "ProjMember", employeeId = manager.Id, status = true };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var members = await _repository.MemberDetails(project.Id);

            Assert.Single(members);
            Assert.Equal("Member", members[0].EmployeeName);
            Assert.Equal("mem@x.com", members[0].Email);
        }

        [Fact]
        public async Task DeleteProject_RemovesEntity_AndReturnsTrue()
        {
            var manager = new EmployeeModel { Name = "pm", Email = "pm@x.com", status = true };
            _context.Employees.Add(manager);
            await _context.SaveChangesAsync();

            var project = new projectModel { Name = "ToDelete", status = true, employeeId = manager.Id };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var ok = await _repository.DeleteProject(project.Id);

            Assert.True(ok);
            Assert.Null(await _context.Projects.FindAsync(project.Id));
        }

        [Fact]
        public async Task UpdateProject_UpdatesFieldsAndReturnsTrue()
        {
            var project = new projectModel { Name = "OldP", status = true, StartDate = DateTime.UtcNow.AddDays(-5) };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var updated = new projectModel { Name = "NewP", status = true, StartDate = DateTime.UtcNow };
            var ok = await _repository.UpdateProject(project.Id, updated);

            Assert.True(ok);

            var fromDb = await _context.Projects.FindAsync(project.Id);
            Assert.Equal("NewP", fromDb.Name);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}