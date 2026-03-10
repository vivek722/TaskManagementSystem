using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface ISubTaskService
{
    Task<bool> AddTask(SubTaskManeg taskManage);
    Task<bool> DeleteTask(int id);
    Task<bool> UpdateTask(int id, SubTaskManeg taskManage);
}
