using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.interfaces;

public interface ITaskServices
{
    Task<TaskManage> AddTask(TaskManage taskManage);
    Task<bool> DeleteTask(int id);

    Task<bool> UpdateTask(int id, TaskManage taskManage);
    Task<TaskManage> GetByIdTask(int id);
    Task<List<TaskManage>> GetAllTask();

    Task<List<TaskManage>> getProjectAllTask(int projectid);
    Task<List<TaskManage>> getEmployeeAllTask(int employeeid);

    Task<int> getEmployeeWithWorkHighstTask(int employeeid);
    Task<int> TotalTaskbyProject(int projectid);
    Task<List<TaskManage>> getEmployeeProirityWiseTask(int employeeId, Proirity proirity);
    Task<List<TaskManage>> getAllProirityWiseTask(Proirity proirity);
    Task<List<TaskManage>> getAllProjectProirityWiseTask(int projectId, Proirity proirity);

    Task<List<TaskManage>> getEmployeeTaskStatusWiseTask(int employeeId, status status);
    Task<List<TaskManage>> getAllProjectStatusWiseTask(status status);
    Task<List<TaskManage>> getSpacificProjectStatusWiseTask(int projectId, status status);
    Task<List<EmployeeTaskStatsDto>> getAllEmployeeTotaltask();
    Task<List<EmployeeTaskStatsDto>> getAllAssinerWithAssignTotaltask();

    Task<int> CompleteTaskCountSpecificProject(int projectid);
    Task<int> PendigTaskCountSpecificProject(int projectid);
    Task<int> PendigTaskCountProject();
    Task<int> CompleteTaskCountProject();
    // Task<List<EmployeeTaskAssinerDto>> getTaskAssinerWithAssienTo();

}
