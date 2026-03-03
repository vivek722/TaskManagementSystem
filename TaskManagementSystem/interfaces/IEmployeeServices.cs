using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface IEmployeeServices
{
    Task<bool> AddEmployee(EmployeeModel employee);
    Task<bool> DeleteEmployee(int id);
    Task<List<EmployeeModel>> GetAllEmployees();
}
