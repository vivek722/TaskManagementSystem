using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface IProjectServices
{
    Task<bool> AddProject(projectModel project);
    Task<bool> DeleteProject(int id);
    Task<List<projectModel>> GetAllProjects();

    Task<projectModel> GetSpecificProjectsWithTask(int id);
    Task<List<projectModel>> GetAllProjectsWithTask();
}
