using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface ISubTaskService
{
    Task<bool> AddTask(SubTaskManeg taskManage);
}
