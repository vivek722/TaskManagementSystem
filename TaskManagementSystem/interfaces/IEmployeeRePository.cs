using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface IEmployeeRePository
{
    Task<bool> AddEmployee(EmployeeModel employee);
    Task<bool> DeleteEmployee(int id);
    Task<List<EmployeeModel>> GetAllEmployees();
    Task<List<EmployeeModel>> GetAllEmployeesWithProjectAndTask();
    Task<EmployeeModel> GetByIdEmployeeWithProjectAndTask(int id);
}
