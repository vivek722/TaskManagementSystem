using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface IEmployeeRePository
{
    Task<bool> AddEmployee(EmployeeModel employee);
    Task<bool> DeleteEmployee(int id);
    Task<bool> UpdateEmployee(int id, EmployeeModel employee);
    Task<EmployeeModel> GetByIdEmployee(int id);
    Task<EmployeeModel> GetByEmilEmployee(string email);
    Task<List<EmployeeModel>> GetAllEmployees();
    Task<List<EmployeeModel>> GetAllEmployeesWithProjectAndTask();
    Task<EmployeeModel> GetByIdEmployeeWithProjectAndTask(int id);
    Task<List<EmployeeModel>> GetEmployeeWhichHaveNotanyTask();
    Task<EmployeeModel> EmployeeWithMostOverDueTask(int id);
    Task<EmployeeModel> EmployeeWithMostCompletedTask(int id);
}
