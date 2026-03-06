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

    public async Task<projectModel> GetSpecificProjectsWithTask(int id)
    {
        return await _appDbContext.Projects.Include(x => x.TaskManages).FirstOrDefaultAsync(x=>x.Id == id);
    }
}
