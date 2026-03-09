using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Repository;

public class EmployeeRepository : IEmployeeRePository
{
    private readonly AppDbContext _appDbContext;
    public EmployeeRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> AddEmployee(EmployeeModel employee)
    {
            await _appDbContext.Employees.AddAsync(employee);
          return await _appDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _appDbContext.Employees.FindAsync(id);
        if (employee == null) return false;

        _appDbContext.Employees.Remove(employee);
        return await _appDbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<EmployeeModel>> GetAllEmployees()
    {
        return await _appDbContext.Employees.AsNoTracking().ToListAsync();
    }

    public Task<List<EmployeeModel>> GetAllEmployeesWithProjectAndTask()
    {
       return _appDbContext.Employees
            .Include(e => e.ProjectModel)
            .ThenInclude(p => p.TaskManages)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<EmployeeModel> GetByEmilEmployee(string email)
    {
        return await _appDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<EmployeeModel> GetByIdEmployee(int id)
    {
        return await _appDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<EmployeeModel> GetByIdEmployeeWithProjectAndTask(int id)
    {
        return _appDbContext.Employees
            .Include(e => e.ProjectModel)
            .ThenInclude(p => p.TaskManages)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> UpdateEmployee(int id, EmployeeModel employee)
    {
        if (id != null)
        {
            var data = await GetByIdEmployee(id);
            data.Name = employee.Name;
            data.Email = employee.Email;
            _appDbContext.Employees.Update(data);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
