using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _appDbContext;
    public TaskRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<bool> AddTask(TaskManage taskManage)
    {
        await _appDbContext.TaskManages.AddAsync(taskManage);
        return await _appDbContext.SaveChangesAsync()>0;
    }

    public async Task<bool> DeleteTask(int id)
    {
         var task = await _appDbContext.TaskManages.FindAsync(id);
        if (task == null) return false;
               _appDbContext.TaskManages.Remove(task);
       return await _appDbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<TaskManage>> GetAllTask()
    {
       return await _appDbContext.TaskManages.AsNoTracking().ToListAsync();
    }

    public async Task<List<TaskManage>> getEmployeeAllTask(int employeeid)
    {
       return await _appDbContext.TaskManages.Include(x=>x.employeeModel).Where(x=>x.employeeModel.Id == employeeid).AsNoTracking().ToListAsync();
    }

    public Task<TaskManage> getEmployeeWithWorkHighstTask(int employeeid)
    {
     
        return null;

    }

    public Task<List<TaskManage>> getProjectAllTask(int projectid)
    {
        return _appDbContext.TaskManages.Include(x=>x.ProjectModel).Where(x=>x.ProjectModel.Id == projectid).AsNoTracking().ToListAsync();
    }

    public async Task<TaskManage> TotalTaskbyProject(int projectid)
    {


        return await _appDbContext.TaskManages.Include(x => x.ProjectModel).GroupBy(x => x.projectId == projectid).ToListAsync().ContinueWith(t => t.Result.Select(g => new TaskManage
        {
            projectId = g.Key ? projectid : 0,
            Id = g.Count()
        }).FirstOrDefault());

    }
}
