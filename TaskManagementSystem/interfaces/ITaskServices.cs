using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface ITaskServices
{
    Task<bool> AddTask(TaskManage taskManage);
    Task<bool> DeleteTask(int id);
    Task<List<TaskManage>> GetAllTask();

    Task<List<TaskManage>> getProjectAllTask(int projectid);
    Task<List<TaskManage>> getEmployeeAllTask(int employeeid);

    Task<TaskManage> getEmployeeWithWorkHighstTask(int employeeid);
    Task<TaskManage> TotalTaskbyProject(int projectid);

}
