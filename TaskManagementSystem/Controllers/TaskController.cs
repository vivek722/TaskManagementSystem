using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.services;

namespace TaskManagementSystem.Controllers;

[ApiController]
public class TaskController : Controller
{
    private readonly ITaskServices taskServices;
    private readonly ISubTaskService subTaskService;
    public TaskController(ITaskServices taskServices, ISubTaskService subTaskService)
    {
        this.taskServices = taskServices;
        this.subTaskService = subTaskService;
    }
    [HttpGet("api/getAllTasks")]
    public async Task<IActionResult> getAllTasks()
    {
        var Alltask = await taskServices.GetAllTask();
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [Authorize(Roles = "Admin,Project_Manager")]
    [HttpPost("api/AddTasks")]
    public async Task<IActionResult> AddTasks([FromBody] TaskDto taskDto)
    {
        if (taskDto == null)
            return BadRequest("Invalid Task Data");
        try
        {
            var task = new TaskManage
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                Assignerid = taskDto.Assignerid,
                assignToId = taskDto.assignToid,
                CreatedAt = DateTime.UtcNow,
                dueDate = taskDto.dueDate,
                employeeId = taskDto.employeeId,
                Proirity = taskDto.Proirity,
                StartDate = taskDto.StartDate,
                EndDate = taskDto.EndDate,
                taskStatus = taskDto.taskStatus,
                projectId = taskDto.projectId
            };

            await taskServices.AddTask(task);

            if (taskDto.subTaskManegs != null && taskDto.subTaskManegs.Any())
            {
                foreach (var stask in taskDto.subTaskManegs)
                {
                    var subtask = new SubTaskManeg
                    {
                        Name = stask.Name,
                        TaskManageid = task.Id,
                        Description = stask.Description,
                        Assignerid = taskDto.Assignerid,
                        assignToid = taskDto.assignToid,
                        dueDate = stask.dueDate,
                        Proirity = stask.Proirity,
                        StartDate = stask.StartDate,
                        EndDate = stask.EndDate,
                        taskStatus = stask.taskStatus
                    };

                    await subTaskService.AddTask(subtask);
                }
            }
            return Ok(task);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet("api/TotalTaskbyProject")]
    public async Task<IActionResult> TotalTaskbyProject(int projectId)
    {
        var Alltask = await taskServices.TotalTaskbyProject(projectId);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }

    [HttpGet("api/getEmployeeAllTasks")]
    public async Task<IActionResult> getEmployeeAllTasks(int employeeId)
    {
        var Alltask = await taskServices.getEmployeeAllTask(employeeId);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }

    [HttpGet("api/getProjectAllTask")]
    public async Task<IActionResult> getProjectAllTask(int projectId)
    {
        var Alltask = await taskServices.getProjectAllTask(projectId);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [HttpGet("api/getEmployeeProirityWiseTask")]
    public async Task<IActionResult> getEmployeeProirityWiseTask(int employeId, Proirity proirity)
    {
        var Alltask = await taskServices.getEmployeeProirityWiseTask(employeId, proirity);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [HttpGet("api/getEmployeeWithWorkHighstTask")]
    public async Task<IActionResult> getEmployeeWithWorkHighstTask(int employeId)
    {
        var Alltask = await taskServices.getEmployeeWithWorkHighstTask(employeId);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }

    [HttpGet("api/getAllTaskBaseOnPrority")]
    public async Task<IActionResult> getAllTaskBaseOnPrority(Proirity proirity)
    {
        var Alltask = await taskServices.getAllProirityWiseTask(proirity);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }

    [HttpGet("api/getProjectWiseHighProrityTask")]
    public async Task<IActionResult> getProjectWiseHighProrityTask(int projectid, Proirity proirity)
    {
        var Alltask = await taskServices.getAllProjectProirityWiseTask(projectid, proirity);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [HttpGet("api/getAllEmployeeTotaltask")]
    public async Task<IActionResult> getAllEmployeeTotaltask()
    {
        var Alltask = await taskServices.getAllEmployeeTotaltask();
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [HttpGet("api/getEmployeeStatusWiseTask")]
    public async Task<IActionResult> getEmployeeStatusWiseTask(int employeId, status status)
    {
        var Alltask = await taskServices.getEmployeeTaskStatusWiseTask(employeId, status);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [HttpGet("api/GetAllStatusWiseTask")]
    public async Task<IActionResult> GetAllStatusWiseTask(status status)
    {
        var Alltask = await taskServices.getAllProjectStatusWiseTask(status);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    [HttpGet("api/GetProjectStatusWiseTask")]
    public async Task<IActionResult> GetProjectStatusWiseTask(int projectid, status status)
    {
        var Alltask = await taskServices.getSpacificProjectStatusWiseTask(projectid, status);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound("task Not found");
    }
    //    [HttpGet("api/getTaskAssignerWithAssignTo")]
    //    public async Task<IActionResult> getTaskAssignerWithAssignTo()
    //    {
    //        var alltask = await taskServices.getTaskAssinerWithAssienTo();
    //        if (alltask != null)
    //        {
    //            return Ok(alltask);
    //        }
    //        return NotFound("task Not found");
    //    }
    //}

}