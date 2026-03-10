using TaskManagementSystem.Data;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Repository;

public class SubTaskRepository : ISubTaskRepository
{
    private readonly AppDbContext _appDbContext;
    public SubTaskRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<bool> AddTask(SubTaskManeg taskManage)
    {
        await _appDbContext.SubTaskManegs.AddAsync(taskManage);
        await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var data = await _appDbContext.SubTaskManegs.FindAsync(id);
        if(data != null)
        {
            _appDbContext.SubTaskManegs.Remove(data);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateTask(int id, SubTaskManeg taskManage)
    {
        var data  = await _appDbContext.SubTaskManegs.FindAsync(id);
        if(data != null)
        {
            data.Name = taskManage.Name;
            data.Description = taskManage.Description;
            data.Assignerid = taskManage.Assignerid;
            data.assignToid = taskManage.assignToid;
            data.Proirity = taskManage.Proirity;
            data.taskStatus = taskManage.taskStatus;
            data.dueDate = taskManage.dueDate;
            data.StartDate= taskManage.StartDate;
            data.EndDate = taskManage.EndDate;

            _appDbContext.SubTaskManegs.UpdateRange(data);
            await _appDbContext.SaveChangesAsync();

            return true;
        }
        return false;
    }
}
