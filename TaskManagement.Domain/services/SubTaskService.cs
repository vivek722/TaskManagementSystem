using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.services;

public class SubTaskService : ISubTaskService
{
    private readonly ISubTaskRepository _subTaskRepository;

    public SubTaskService(ISubTaskRepository subTaskRepository)
    {
        _subTaskRepository = subTaskRepository;
    }
    public async Task<bool> AddTask(SubTaskManeg taskManage)
    {
        return await _subTaskRepository.AddTask(taskManage);
    }

    public async Task<bool> DeleteTask(int id)
    {
        return await _subTaskRepository.DeleteTask(id);
    }

    public async Task<bool> UpdateTask(int id, SubTaskManeg taskManage)
    {
        return await _subTaskRepository.UpdateTask(id, taskManage);
    }
}
