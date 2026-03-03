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
   
}
