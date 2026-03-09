using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

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
        return await _appDbContext.Projects.AsNoTracking().ToListAsync();
    }

    public async Task<List<projectModel>> GetAllProjectsWithTask()
    {
        return await _appDbContext.Projects.Include(x => x.TaskManages).AsNoTracking().ToListAsync();
    }

    public async Task<projectModel> GetByIdProjet(int id)
    {
        return await _appDbContext.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<projectModel> GetSpecificProjectsWithTask(int id)
    {
        return await _appDbContext.Projects.Include(x => x.TaskManages).FirstOrDefaultAsync(x=>x.Id == id);
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
