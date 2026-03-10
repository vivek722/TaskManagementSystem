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

    public Task<EmployeeModel> EmployeeWithMostCompletedTask(int id)
    {
        return _appDbContext.Employees
            .Include(e => e.ProjectModel)
            .ThenInclude(p => p.TaskManages)
            .Where(e => e.ProjectModel.Any(p => p.TaskManages.Any(t => t.taskStatus == status.Completed && t.status != false)))
            .OrderByDescending(e => e.ProjectModel.Sum(p => p.TaskManages.Count(t => t.taskStatus == status.Completed && t.status != false)))
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public Task<EmployeeModel> EmployeeWithMostOverDueTask(int id)
    {
        return _appDbContext.Employees
            .Include(e => e.ProjectModel)
            .ThenInclude(p => p.TaskManages)
            .Where(e => e.ProjectModel.Any(p => p.TaskManages.Any(t => t.taskStatus == status.OverDue && t.status != false)))
            .OrderByDescending(e => e.ProjectModel.Sum(p => p.TaskManages.Count(t => t.taskStatus == status.OverDue && t.status != false)))
            .FirstOrDefaultAsync(e => e.Id == id);
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
            .Where(x => x.status != false)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<EmployeeModel> GetByEmilEmployee(string email)
    {
        return await _appDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && x.status != false);
    }

    public async Task<EmployeeModel> GetByIdEmployee(int id)
    {
        return await _appDbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.status != false);
    }

    public Task<EmployeeModel> GetByIdEmployeeWithProjectAndTask(int id)
    {
        return _appDbContext.Employees
            .Include(e => e.ProjectModel)
            .ThenInclude(p => p.TaskManages)
            .Where(x => x.status != false)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<EmployeeModel>> GetEmployeeWhichHaveNotanyTask()
    {
        var result = await _appDbContext.Employees
            .GroupJoin(
                _appDbContext.TaskManages,
                emp => emp.Id,
                task => task.assignToId,
                (emp, tasks) => new { emp, tasks }
            )
            .Where(x => !x.tasks.Any())
            .Select(x => new EmployeeModel
            {
                Id = x.emp.Id,
                Name = x.emp.Name,
                role = x.emp.role,
            })
            .ToListAsync();

        return result;
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
