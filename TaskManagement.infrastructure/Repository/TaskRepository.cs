using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _appDbContext;
    public TaskRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<TaskManage> AddTask(TaskManage taskManage)
    {
         await _appDbContext.TaskManages.AddAsync(taskManage);
          await _appDbContext.SaveChangesAsync();

        return taskManage;
    }

    public async Task<int> CompleteTaskCountSpecificProject(int projectid)
    {
        return await _appDbContext.TaskManages
             .Where(x => x.projectId == projectid && x.taskStatus == status.Completed && x.status != false)
             .CountAsync();
    }

    public async Task<int> CompleteTaskCountProject()
    {
        return await _appDbContext.TaskManages
            .Where(x => x.taskStatus == status.Completed && x.status != false)
            .CountAsync();
    }

    public async Task<bool> DeleteTask(int id)
    {
             var task = await _appDbContext.TaskManages.FindAsync(id);
            if (task == null) return false;
                   _appDbContext.TaskManages.Remove(task);
           return await _appDbContext.SaveChangesAsync() > 0;
    }

    public Task<List<EmployeeTaskStatsDto>> getAllAssinerWithAssignTotaltask()
    {
        var assignerStats = _appDbContext.TaskManages
            .GroupJoin(
                _appDbContext.Employees,
                task => task.Assignerid,
                emp => emp.Id,
                (task, emps) => new EmployeeTaskStatsDto
                {
                    EmployeeName = emps.FirstOrDefault().Name,
                    TotalTasks = emps.Count(),
                })
            .OrderByDescending(x => x.TotalTasks)
            .ToListAsync();
        return assignerStats;
    }

    public async Task<List<EmployeeTaskStatsDto>> getAllEmployeeTotaltask()
    {
        var employeeStats = await _appDbContext.Employees
            .GroupJoin(
                _appDbContext.TaskManages,
                emp => emp.Id,
                task => task.assignToId,
                (emp, tasks) => new EmployeeTaskStatsDto
                {
                    EmployeeName = emp.Name,
                    TotalTasks = tasks.Count(),
                })
            .OrderByDescending(x => x.TotalTasks)
            .ToListAsync();

        return employeeStats;
    }

    public async Task<List<TaskManage>> getAllProirityWiseTask(Proirity proirity)
    {
        return await _appDbContext.TaskManages.Where(x => x.Proirity == proirity && x.status != false).AsNoTracking().ToListAsync();
    }

    public async Task<List<TaskManage>> getAllProjectProirityWiseTask(int projectId, Proirity proirity)
    {
        return await _appDbContext.TaskManages.Where(x => x.Proirity == proirity && x.projectId == projectId && x.status != false).AsNoTracking().ToListAsync();
    }

    public async Task<List<TaskManage>> getAllProjectStatusWiseTask(status status)
    {
        return await _appDbContext.TaskManages.Where(x => x.taskStatus == status && x.status != false).ToListAsync();
    }

    public async Task<List<TaskManage>> GetAllTask()
    {
       return await _appDbContext.TaskManages.Include(x => x.SubTasks).AsNoTracking().ToListAsync();
    }

    public async Task<TaskManage> GetByIdTask(int id)
    {
        return await _appDbContext.TaskManages.Include(x=>x.SubTasks).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.status != false);
    }

    public async Task<List<TaskManage>> getEmployeeAllTask(int employeeid)
    {
       return await _appDbContext.TaskManages.Include(x=>x.employeeModel).Where(x=>x.employeeModel.Id == employeeid && x.status != false).AsNoTracking().ToListAsync();
    }

    public async Task<List<TaskManage>> getEmployeeProirityWiseTask(int employeeId, Proirity proirity)
    {
        return await _appDbContext.TaskManages.Where(x => x.Id == employeeId && x.Proirity == proirity).ToListAsync();
    }

    public async Task<List<TaskManage>> getEmployeeTaskStatusWiseTask(int employeeId, status status)
    {
        return await _appDbContext.TaskManages.Where(x => x.Id == employeeId && x.taskStatus == status && x.status != false).ToListAsync();
    }

    public async Task<int> getEmployeeWithWorkHighstTask(int employeeid)
    {
        return await _appDbContext.TaskManages
         .Where(x => x.employeeId == employeeid && x.status != false)
         .CountAsync();
    }

    public Task<List<TaskManage>> getProjectAllTask(int projectid)
    {
        return _appDbContext.TaskManages.Include(x=>x.ProjectModel).Where(x=>x.ProjectModel.Id == projectid && x.status != false).AsNoTracking().ToListAsync();
    }

    public async Task<List<TaskManage>> getSpacificProjectStatusWiseTask(int projectId, status status)
    {
        return await _appDbContext.TaskManages.Where(x => x.projectId == projectId && x.taskStatus == status && x.status != false).ToListAsync();
    }

    public async Task<int> PendigTaskCountSpecificProject(int projectid)
    {
        return await _appDbContext.TaskManages
            .Where(x => x.projectId == projectid && x.taskStatus == status.Pending && x.status != false)
            .CountAsync();
    }

    public async Task<int> PendigTaskCountProject()
    {
        return await _appDbContext.TaskManages
            .Where(x => x.taskStatus == status.Pending && x.status != false)
            .CountAsync();
    }

    public async Task<int> TotalTaskbyProject(int projectid)
    {
        return await _appDbContext.TaskManages
        .Where(x => x.projectId == projectid && x.status != false)
        .CountAsync();
    }

    public async Task<bool> UpdateTask(int id, TaskManage taskManage)
    {
        var data = await GetByIdTask(id);
        if(data != null)
            {
            data.Name = taskManage.Name;
            data.Description = taskManage.Description;
            data.dueDate = taskManage.dueDate;
            data.StartDate = taskManage.StartDate;
            data.EndDate = taskManage.EndDate;
            data.Assignerid = taskManage.Assignerid;
            data.assignToId = taskManage.assignToId;
            data.taskStatus = taskManage.taskStatus;
            data.Proirity = taskManage.Proirity;

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
