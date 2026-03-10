using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagementSystem.DataResponseModel;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class ProjectController : Controller
{
    private readonly IProjectServices projectServices;
    private readonly IMapper _mapper;
    public ProjectController(IProjectServices projectServices ,IMapper mapper)
    {
        this.projectServices = projectServices;
        _mapper = mapper;
    }
    [HttpGet("GetProjects")]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await projectServices.GetAllProjects();
        if (projects != null)
        {
            return Ok(new DataSuccessResponseModel<List<projectModel>>() { data = projects, Message = "GetAllProjects successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Project not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddProjects")]
    public async Task<IActionResult> AddProjects([FromBody] projectDto projectDto)
    {
        if (projectDto != null)
        {
            var project = _mapper.Map<projectModel>(projectDto);

            project.CreatedAt = DateTime.Now;
            project.UpdatedAt = null;
            project.status = true;
            project.employeeId = projectDto.ProjectManagerId;
            
            var result = await projectServices.AddProject(project);
            return Ok(new DataSuccessResponseModel<projectModel>() { data = project, Message = "AddProjects successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Project Data Is not valid",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("GetAllPRojectWithTask")]
    public async Task<IActionResult> GetAllPRojectWithTask()
    {
        var projects = await projectServices.GetAllProjectsWithTask();
        if(projects != null)
        {
            return Ok(new DataSuccessResponseModel<List<projectModel>>() { data = projects, Message = "GetAllPRojectWithTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Project not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("DeleteProject")]
    public async Task<IActionResult> Delete(int projectId)
    {
        var Data = await projectServices.DeleteProject(projectId);
        if (Data == true)
        {
            return Ok(Data);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Project not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetspecificProjectWithTask")]
    public async Task<IActionResult> getSpecificProjectAllTask(int id)
    {

        var project = await projectServices.GetSpecificProjectsWithTask(id);
        if(project != null)
        {
            return Ok(new DataSuccessResponseModel<projectModel>() { data = project, Message = "getSpecificProjectAllTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Project not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetProjectMemberdetails")]
    public async Task<IActionResult> GetProjectMemberdetails(int Projectid)
    {
        var projectMember = await projectServices.MemberDetails(Projectid);
        if (projectMember != null)
        {
            return Ok(new DataSuccessResponseModel<List<MemberDetails>>() { data = projectMember, Message = "GetProjectMemberdetails successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Project not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getProjectWithOverDueTask")]
    public async Task<IActionResult> GetProjectWithOverDueTask(int Projectid)
    {
            var project = await projectServices.GetProjectWithOverDueTask(Projectid);
            if (project != null)
            {
                 return Ok(new DataSuccessResponseModel<projectModel>() { data = project, Message = "GetProjectWithOverDueTask successfully", StatusCode = StatusCodes.Status200OK });
            }
            return NotFound(new DataErrorResponseModel
            {
                Message = "No OverDue Task Found",
                StatusCode = StatusCodes.Status404NotFound
            });
    }
    [HttpPut("UpdateProject")]
    public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] projectDto projectDto)
    {
            if (projectDto != null)
            {
                var project = _mapper.Map<projectModel>(projectDto);

                project.CreatedAt = project.CreatedAt;
                project.UpdatedAt = DateTime.Now;
                project.status = true;
                project.employeeId = projectDto.ProjectManagerId;

                var result = await projectServices.UpdateProject(id, project);
            return Ok(new DataSuccessResponseModel<projectModel>() { data = project, Message = "UpdateProject successfully", StatusCode = StatusCodes.Status200OK });
        }
            return NotFound(new DataErrorResponseModel
            {
                Message = "invalid Project Data",
                StatusCode = StatusCodes.Status404NotFound
            });
    }
}
