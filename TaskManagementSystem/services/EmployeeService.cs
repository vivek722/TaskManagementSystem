using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.services;

public class EmployeeService : IEmployeeServices
{
    private readonly IEmployeeRePository _employeeRePository;

    public EmployeeService(IEmployeeRePository employeeRePository)
    {
        _employeeRePository = employeeRePository;
    }
    public async Task<bool> AddEmployee(EmployeeModel employee)
    {
        return await _employeeRePository.AddEmployee(employee);
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        return await _employeeRePository.DeleteEmployee(id);
    }

    public async Task<List<EmployeeModel>> GetAllEmployees()
    {
        return await _employeeRePository.GetAllEmployees();
    }

    public async Task<List<EmployeeModel>> GetAllEmployeesWithProjectAndTask()
    {
        return await _employeeRePository.GetAllEmployeesWithProjectAndTask();
    }

    public async Task<EmployeeModel> GetByIdEmployeeWithProjectAndTask(int id)
    {
      return await _employeeRePository.GetByIdEmployeeWithProjectAndTask(id);
    }
}
