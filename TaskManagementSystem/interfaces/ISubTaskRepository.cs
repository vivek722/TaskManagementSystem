using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface ISubTaskRepository
{
    Task<bool> AddTask(SubTaskManeg taskManage);
}
