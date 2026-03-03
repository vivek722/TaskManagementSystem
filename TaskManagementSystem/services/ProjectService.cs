using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.services;

public class ProjectService : IProjectServices
{
    private readonly IProjectRepository _projectRepository;
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public  async Task<bool> AddProject(projectModel project)
    {
        return await _projectRepository.AddProject(project);
    }

    public async Task<bool> DeleteProject(int id)
    {
        return await _projectRepository.DeleteProject(id);
    }

    public async Task<List<projectModel>> GetAllProjects()
    {
        return await _projectRepository.GetAllProjects();
    }
}
