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
                employeeId = projectDto.employeeId
            };
            var result = await projectServices.AddProject(project);
            return Ok(result);
        }
        return BadRequest("Invalid Project Data");
    }
}
