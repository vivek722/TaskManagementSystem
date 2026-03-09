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
}
