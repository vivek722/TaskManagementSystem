using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface IProjectRepository
{
    Task<bool> AddProject(projectModel project);
    Task<bool> DeleteProject(int id);
    Task<List<projectModel>> GetAllProjects();
}
