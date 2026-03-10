using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.services;

public class TaskService : ITaskServices              
{
    private readonly ITaskRepository taskRepository;
    public TaskService(ITaskRepository taskRepository)
    {
        this.taskRepository = taskRepository;
    }
    public async Task<TaskManage> AddTask(TaskManage taskManage)
    {
        return await taskRepository.AddTask(taskManage);    
    }

    public Task<int> CompleteTaskCountProject()
    {
        return taskRepository.CompleteTaskCountProject();
    }

    public Task<int> CompleteTaskCountSpecificProject(int projectid)
    {
        return taskRepository.CompleteTaskCountSpecificProject(projectid);
    }

    public async Task<bool> DeleteTask(int id)
    {
        return await taskRepository.DeleteTask(id);
    }

    public async Task<List<EmployeeTaskStatsDto>> getAllAssinerWithAssignTotaltask()
    {
        return await taskRepository.getAllAssinerWithAssignTotaltask();
    }

    public async Task<List<EmployeeTaskStatsDto>> getAllEmployeeTotaltask()
    {
      return await taskRepository.getAllEmployeeTotaltask();
    }

    public async Task<List<TaskManage>> getAllProirityWiseTask(Proirity proirity)
    {
        return await taskRepository.getAllProirityWiseTask(proirity);
    }

    public Task<List<TaskManage>> getAllProjectProirityWiseTask(int projectId, Proirity proirity)
    {
        return taskRepository.getAllProjectProirityWiseTask(projectId, proirity);
    }

    public async Task<List<TaskManage>> getAllProjectStatusWiseTask(status status)
    {
        return await taskRepository.getAllProjectStatusWiseTask(status);
    }

    public async Task<List<TaskManage>> GetAllTask()
    {
        return await taskRepository.GetAllTask();
    }

    public async Task<TaskManage> GetByIdTask(int id)
    {
        return await taskRepository.GetByIdTask(id);
    }

    public async Task<List<TaskManage>> getEmployeeAllTask(int employeeid)
    {
        return await taskRepository.getEmployeeAllTask(employeeid);
    }

    public async Task<List<TaskManage>> getEmployeeProirityWiseTask(int employeeId, Proirity proirity)
    {
        return await taskRepository.getEmployeeProirityWiseTask(employeeId, proirity);
    }

    public async Task<List<TaskManage>> getEmployeeTaskStatusWiseTask(int employeeId, status status)
    {
        return await taskRepository.getEmployeeTaskStatusWiseTask(employeeId, status);
    }

    public async Task<int> getEmployeeWithWorkHighstTask(int employeeid)
    {
        return await taskRepository.getEmployeeWithWorkHighstTask(employeeid);
    }

    public async Task<List<TaskManage>> getProjectAllTask(int projectid)
    {
        return await taskRepository.getProjectAllTask(projectid);
    }

    public async Task<List<TaskManage>> getSpacificProjectStatusWiseTask(int projectId, status status)
    {
        return await taskRepository.getSpacificProjectStatusWiseTask(projectId, status);
    }

    public async Task<int> PendigTaskCountProject()
    {
        return await taskRepository.PendigTaskCountProject();
    }

    public async Task<int> PendigTaskCountSpecificProject(int projectid)
    {
        return await taskRepository.PendigTaskCountSpecificProject(projectid);
    }

    //public async Task<List<EmployeeTaskAssinerDto>> getTaskAssinerWithAssienTo()
    //{
    //   return await taskRepository.getTaskAssinerWithAssienTo();
    //}

    public async Task<int> TotalTaskbyProject(int projectid)
    {
        return await taskRepository.TotalTaskbyProject(projectid);
    }

    public async Task<bool> UpdateTask(int id, TaskManage taskManage)
    {
        return await taskRepository.UpdateTask(id, taskManage);
    }
}
