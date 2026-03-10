using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.interfaces;

public interface IProjectServices
{
    Task<bool> AddProject(projectModel project);
    Task<bool> DeleteProject(int id);
    Task<projectModel> GetByIdProjet(int id);
    Task<bool> UpdateProject(int id, projectModel project);
    Task<List<projectModel>> GetAllProjects();
    Task<projectModel> GetSpecificProjectsWithTask(int id);
    Task<List<projectModel>> GetAllProjectsWithTask();
    Task<List<MemberDetails>> MemberDetails(int id);
    Task<projectModel> GetProjectWithOverDueTask(int id);
}
