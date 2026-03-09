using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Controllers;

[ApiController]
public class ProjectController : Controller
{
    private readonly IProjectServices projectServices;
    public ProjectController(IProjectServices projectServices)
    {
        this.projectServices = projectServices;
    }
    [HttpGet("api/GetProjects")]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await projectServices.GetAllProjects();
        if (projects != null)
        {
            return Ok(projects);
        }
        return NotFound("No Projects Found");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("api/AddProjects")]
    public async Task<IActionResult> AddProjects([FromBody] Projectdto projectDto)
    {
        if (projectDto != null)
        {
            var project = new projectModel
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                employeeId = projectDto.ProjectManagerId
            };
            var result = await projectServices.AddProject(project);
            return Ok(result);
        }
        return BadRequest("Invalid Project Data");
    }
    [HttpGet("api/GetAllPRojectWithTask")]
    public async Task<IActionResult> GetAllPRojectWithTask()
    {
        var Data = await projectServices.GetAllProjectsWithTask();
        if(Data != null)
        {
            return Ok(Data);
        }
        return NotFound("any Project not found");
    }
    [HttpGet("api/GetspecificProjectWithTask")]
    public async Task<IActionResult> getSpecificProjectAllTask(int id)
    {

        var data = projectServices.GetSpecificProjectsWithTask(id);
        if(data != null)
        {
            return Ok(data);
        }
        return NoContent();
    }
}
