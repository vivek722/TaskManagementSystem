using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.services;

public class TaskService : ITaskServices
{
    private readonly ITaskRepository taskRepository;
    public TaskService(ITaskRepository taskRepository)
    {
        this.taskRepository = taskRepository;
    }
    public async Task<bool> AddTask(TaskManage taskManage)
    {
        return await taskRepository.AddTask(taskManage);    
    }

    public async Task<bool> DeleteTask(int id)
    {
        return await taskRepository.DeleteTask(id);
    }

    public async Task<List<TaskManage>> GetAllTask()
    {
        return await taskRepository.GetAllTask();
    }

    public async Task<List<TaskManage>> getEmployeeAllTask(int employeeid)
    {
        return await taskRepository.getEmployeeAllTask(employeeid);
    }

    public async Task<TaskManage> getEmployeeWithWorkHighstTask(int employeeid)
    {
        return await taskRepository.getEmployeeWithWorkHighstTask(employeeid);
    }

    public Task<List<TaskManage>> getProjectAllTask(int projectid)
    {
        return taskRepository.getProjectAllTask(projectid);
    }

    public async Task<TaskManage> TotalTaskbyProject(int projectid)
    {
        return await taskRepository.TotalTaskbyProject(projectid);
    }
}
