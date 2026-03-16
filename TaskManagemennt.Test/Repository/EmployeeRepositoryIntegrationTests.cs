using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Repository;
using TaskManagementSystem.Model;
using TaskManagementSystem.Data;
using Xunit;

namespace TaskManagemennt.Test.Repository
{
    public class EmployeeRepositoryIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly EmployeeRepository _repository;

        public EmployeeRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new EmployeeRepository(_context);
        }

        [Fact]
        public async Task AddEmployee_PersistsAndReturnsTrue()
        {
            var emp = new EmployeeModel { Name = "RepoEmp", Email = "repo@x.com", status = true };

            var ok = await _repository.AddEmployee(emp);

            Assert.True(ok);
            Assert.NotEqual(0, emp.Id);

            var fromDb = await _context.Employees.FindAsync(emp.Id);
            Assert.Equal("RepoEmp", fromDb.Name);
        }

        [Fact]
        public async Task GetByIdEmployee_ReturnsEmployee_WhenExists()
        {
            var emp = new EmployeeModel { Name = "G1", Email = "g1@x.com", status = true };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            var res = await _repository.GetByIdEmployee(emp.Id);

            Assert.NotNull(res);
            Assert.Equal("G1", res.Name);
        }

        [Fact]
        public async Task DeleteEmployee_RemovesEntity_AndReturnsTrue()
        {
            var emp = new EmployeeModel { Name = "ToDelete", Email = "td@x.com", status = true };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            var ok = await _repository.DeleteEmployee(emp.Id);

            Assert.True(ok);
            Assert.Null(await _context.Employees.FindAsync(emp.Id));
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsAllAddedEmployees()
        {
            _context.Employees.AddRange(new EmployeeModel { Name = "A", status = true }, new EmployeeModel { Name = "B", status = true });
            await _context.SaveChangesAsync();

            var list = await _repository.GetAllEmployees();

            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetEmployeeWhichHaveNotanyTask_ReturnsOnlyEmployeesWithoutTasks()
        {
            var empWithTask = new EmployeeModel { Name = "WithTask", Email = "w@x.com", status = true };
            var empNoTask = new EmployeeModel { Name = "NoTask", Email = "n@x.com", status = true };
            _context.Employees.AddRange(empWithTask, empNoTask);
            await _context.SaveChangesAsync();

            var task = new TaskManage { Name = "T1", assignToId = empWithTask.Id, projectId = 1, status = true };
            _context.TaskManages.Add(task);
            await _context.SaveChangesAsync();

            var result = await _repository.GetEmployeeWhichHaveNotanyTask();

            Assert.Single(result);
            Assert.Equal("NoTask", result[0].Name);
        }

        [Fact]
        public async Task UpdateEmployee_UpdatesFieldsAndReturnsTrue()
        {
            var emp = new EmployeeModel { Name = "Old", Email = "old@x.com", status = true };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            var updated = new EmployeeModel { Name = "New", Email = "new@x.com" };

            var ok = await _repository.UpdateEmployee(emp.Id, updated);

            Assert.True(ok);

            var fromDb = await _context.Employees.FindAsync(emp.Id);
            Assert.Equal("New", fromDb.Name);
            Assert.Equal("new@x.com", fromDb.Email);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}