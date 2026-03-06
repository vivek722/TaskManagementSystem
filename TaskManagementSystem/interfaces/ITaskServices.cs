using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.interfaces;

public interface ITaskServices
{
    Task<bool> AddTask(TaskManage taskManage);
    Task<bool> DeleteTask(int id);
    Task<List<TaskManage>> GetAllTask();

    Task<List<TaskManage>> getProjectAllTask(int projectid);
    Task<List<TaskManage>> getEmployeeAllTask(int employeeid);

    Task<int> getEmployeeWithWorkHighstTask(int employeeid);
    Task<int> TotalTaskbyProject(int projectid);
    Task<List<TaskManage>> getEmployeeProirityWiseTask(int employeeId,Proirity proirity);
    Task<List<EmployeeTaskStatsDto>> getAllEmployeeTotaltask();
    Task<List<EmployeeTaskAssinerDto>> getTaskAssinerWithAssienTo();

}
