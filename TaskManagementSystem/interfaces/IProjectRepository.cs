using TaskManagementSystem.Model;

namespace TaskManagementSystem.interfaces;

public interface IProjectRepository
{
    Task<bool> AddProject(projectModel project);
    Task<bool> DeleteProject(int id);
    Task<projectModel> GetByIdProjet(int id);
    Task<bool> UpdateProject(int id, projectModel project);
    Task<List<projectModel>> GetAllProjects();
    Task<projectModel> GetSpecificProjectsWithTask(int id);
    Task<List<projectModel>> GetAllProjectsWithTask();

}
