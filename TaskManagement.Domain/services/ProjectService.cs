using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

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

    public async Task<List<projectModel>> GetAllProjectsWithTask()
    {
        return await _projectRepository.GetAllProjectsWithTask();
    }

    public async Task<projectModel> GetByIdProjet(int id)
    {
        return await _projectRepository.GetByIdProjet(id);
    }

    public async Task<projectModel> GetProjectWithOverDueTask(int id)
    {
       return await _projectRepository.GetProjectWithOverDueTask(id);
    }

    public async Task<projectModel> GetSpecificProjectsWithTask(int id)
    {
        return await _projectRepository.GetSpecificProjectsWithTask(id);
    }

    public async Task<List<MemberDetails>> MemberDetails(int id)
    {
        return await _projectRepository.MemberDetails(id);
    }

    public async Task<bool> UpdateProject(int id, projectModel project)
    {
        return await _projectRepository.UpdateProject(id, project);
    }
}
