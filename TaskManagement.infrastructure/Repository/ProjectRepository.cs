using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _appDbContext;
    public ProjectRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<bool> AddProject(projectModel project)
    {
        await _appDbContext.Projects.AddAsync(project);
        return await _appDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteProject(int id)
    {
        var project = await _appDbContext.Projects.FindAsync(id);
        if (project == null) return false;

        _appDbContext.Projects.Remove(project);
        return await _appDbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<projectModel>> GetAllProjects()
    {
        return await _appDbContext.Projects.AsNoTracking().Where(x => x.status != false).ToListAsync();
    }

    public async Task<List<projectModel>> GetAllProjectsWithTask()
    {
        return await _appDbContext.Projects.Include(x => x.TaskManages).Where(x=>x.status != false).AsNoTracking().ToListAsync();
    }

    public async Task<projectModel> GetByIdProjet(int id)
    {
        return await _appDbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.status != false);
    }

    public Task<projectModel> GetProjectWithOverDueTask(int id)
    {
       return _appDbContext.Projects
            .Include(p => p.TaskManages.Where(t => t.dueDate < DateTime.Now && t.taskStatus != status.Completed && t.status != false))
            .FirstOrDefaultAsync(p => p.Id == id && p.status != false);
    }

    public async Task<projectModel> GetSpecificProjectsWithTask(int id)
    {
        return await _appDbContext.Projects.Include(x => x.TaskManages).FirstOrDefaultAsync(x=>x.Id == id && x.status != false);
    }

    public async Task<List<MemberDetails>> MemberDetails(int Prjectid)
    {
        return await _appDbContext.Projects
            .Where(e => e.Id == Prjectid)
            .Include(e => e.employeeModel)
            .Select(e => new MemberDetails
            {
                EmployeeName = e.employeeModel.Name,
                Email = e.employeeModel.Email,
                role = e.employeeModel.role
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateProject(int id, projectModel project)
    {
        var Data = await GetByIdProjet(id);
        if(Data != null)
        {
            Data.Name = project.Name;
            Data.status = project.status;
            Data.StartDate = project.StartDate;
            Data.Description = project.Description;
            Data.EndDate = project.EndDate;

            _appDbContext.Projects.Update(Data);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
