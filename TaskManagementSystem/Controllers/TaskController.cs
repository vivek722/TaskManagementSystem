using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DataResponseModel;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.ResponseDto;
using TaskManagementSystem.services;

namespace TaskManagementSystem.Controllers;

[ApiController]
public class TaskController : Controller
{
    private readonly ITaskServices taskServices;
    private readonly ISubTaskService subTaskService;
    private readonly IMapper _mapper;
    public TaskController(ITaskServices taskServices, ISubTaskService subTaskService, IMapper mapper)
    {
        this.taskServices = taskServices;
        this.subTaskService = subTaskService;
        _mapper = mapper;
    }
    [HttpGet("getAllTasks")]
    public async Task<IActionResult> getAllTasks()
    {
        var Alltask = await taskServices.GetAllTask();
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getAllTasks successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [Authorize(Roles = "Admin,Project_Manager")]
    [HttpPost("AddTasks")]
    public async Task<IActionResult> AddTasks([FromBody] TaskDto taskDto)
    {
        if (taskDto == null)
            return NotFound(new DataErrorResponseModel
            {
                Message = "Task not found",
                StatusCode = StatusCodes.Status404NotFound
            });

        var task = _mapper.Map<TaskManage>(taskDto);

        var taskdata = await taskServices.AddTask(task);
        if (taskDto.subTaskManegs != null && taskDto.subTaskManegs.Count > 0)
        {
            foreach (var sub in taskDto.subTaskManegs)
            {
                    var subTask = _mapper.Map<SubTaskManeg>(sub);

                    subTask.TaskManageid = taskdata.Id;

                    await subTaskService.AddTask(subTask);
                }
            }
        return Ok(new DataSuccessResponseModel<TaskManage>() { data = taskdata, Message = "AddTasks successfully", StatusCode = StatusCodes.Status200OK });
    }

    [Authorize(Roles = "Admin,Project_Manager")]
    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> DeleteTask(int TaskId)
    {
        var DeletedTask = await taskServices.DeleteTask(TaskId);
        if (DeletedTask == true)
        {
            return Ok(new DataSuccessResponseModel<bool>() { data = DeletedTask, Message = "DeleteTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [Authorize(Roles = "Admin,Project_Manager")]
    [HttpPut("UpdateTask/{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto taskDto)
    {
        if (taskDto == null)
            return NotFound(new DataErrorResponseModel
            {
                Message = "Task not found",
                StatusCode = StatusCodes.Status404NotFound
            });
            var task = _mapper.Map<TaskManage>(taskDto);

            await taskServices.UpdateTask(id,task);

            if (taskDto.subTaskManegs != null && taskDto.subTaskManegs.Count > 0)
            {
                foreach (var sub in taskDto.subTaskManegs)
                {
                    var subTask = _mapper.Map<SubTaskManeg>(sub);

                    subTask.TaskManageid = id;

                    await subTaskService.UpdateTask(subTask.Id,subTask);
                }
            }
            return Ok(new DataSuccessResponseModel<TaskManage>() { data = task, Message = "UpdateTask successfully", StatusCode = StatusCodes.Status200OK }); 
    }
    [HttpGet("TotalTaskbyProject")]
    public async Task<IActionResult> TotalTaskbyProject(int projectId)
    {
        var Alltask = await taskServices.TotalTaskbyProject(projectId);
        if (Alltask != null)
        {
            return Ok(Alltask);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getEmployeeAllTasks")]
    public async Task<IActionResult> getEmployeeAllTasks(int employeeId)
    {
        var Alltask = await taskServices.getEmployeeAllTask(employeeId);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getEmployeeAllTasks successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getProjectAllTask")]
    public async Task<IActionResult> getProjectAllTask(int projectId)
    {
        var Alltask = await taskServices.getProjectAllTask(projectId);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getProjectAllTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound("task Not found");
    }
    [HttpGet("getEmployeeProirityWiseTask")]
    public async Task<IActionResult> getEmployeeProirityWiseTask(int employeId, Proirity proirity)
    {
        var Alltask = await taskServices.getEmployeeProirityWiseTask(employeId, proirity);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getEmployeeProirityWiseTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound("task Not found");
    }
    [HttpGet("getEmployeeWithWorkHighstTask")]
    public async Task<IActionResult> getEmployeeWithWorkHighstTask(int employeId)
    {
        var Alltask = await taskServices.getEmployeeWithWorkHighstTask(employeId);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<int>() { data = Alltask, Message = "getEmployeeWithWorkHighstTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getTaskbyId")]
    public async Task<IActionResult> getTaskbyId(int TaksId)
    {
        var Alltask = await taskServices.GetByIdTask(TaksId);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<TaskManage>() { data = Alltask, Message = "getTaskbyId successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getAllTaskBaseOnPrority")]
    public async Task<IActionResult> getAllTaskBaseOnPrority(Proirity proirity)
    {
        var Alltask = await taskServices.getAllProirityWiseTask(proirity);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getAllTaskBaseOnPrority successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getProjectWiseHighProrityTask")]
    public async Task<IActionResult> getProjectWiseHighProrityTask(int projectid, Proirity proirity)
    {
        var Alltask = await taskServices.getAllProjectProirityWiseTask(projectid, proirity);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getProjectWiseHighProrityTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("getAllEmployeeTotaltask")]
    public async Task<IActionResult> getAllEmployeeTotaltask()
    {
        var Alltask = await taskServices.getAllEmployeeTotaltask();
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<EmployeeTaskStatsDto>>() { data = Alltask, Message = "getAllEmployeeTotaltask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("getEmployeeStatusWiseTask")]
    public async Task<IActionResult> getEmployeeStatusWiseTask(int employeId, status status)
    {
        var Alltask = await taskServices.getEmployeeTaskStatusWiseTask(employeId, status);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "getAllEmployeeTotaltask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetAllStatusWiseTask")]
    public async Task<IActionResult> GetAllStatusWiseTask(status status)
    {
        var Alltask = await taskServices.getAllProjectStatusWiseTask(status);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "GetAllStatusWiseTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetProjectStatusWiseTask")]
    public async Task<IActionResult> GetProjectStatusWiseTask(int projectid, status status)
    {
        var Alltask = await taskServices.getSpacificProjectStatusWiseTask(projectid, status);
        if (Alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<TaskManage>>() { data = Alltask, Message = "GetProjectStatusWiseTask  successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("getAllAssinerWithAssignTotaltask")]
    public async Task<IActionResult> getAllAssinerWithAssignTotaltask()
    {
        var alltask = await taskServices.getAllAssinerWithAssignTotaltask();
        if (alltask != null)
        {
            return Ok(new DataSuccessResponseModel<List<EmployeeTaskStatsDto>>() { data = alltask, Message = "GetProjectStatusWiseTask  successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetAllProjectTotalCompletedTask")]
    public async Task<IActionResult> GetAllProjectTotalCompletedTask()
    {
        var alltask = await taskServices.CompleteTaskCountProject();
        if (alltask != null)
        {
            return Ok(alltask);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetAllSpecificProjectTotalCompletedTask/{projectid}")]
    public async Task<IActionResult> GetAllSpecificProjectTotalCompletedTask(int projectid)
    {
        var alltask = await taskServices.CompleteTaskCountSpecificProject(projectid);
        if (alltask != null)
        {
            return Ok(alltask);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetAllProjectTotalPendingTask")]
    public async Task<IActionResult> GetAllProjectTotalPendingTask()
    {
        var alltask = await taskServices.PendigTaskCountProject();
        if (alltask != null)
        {
            return Ok(alltask);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("GetAllSpecificProjectTotalPendingTask/{projectid}")]
    public async Task<IActionResult> GetAllSpecificProjectTotalPendingTask(int projectid)
    {
        var alltask = await taskServices.PendigTaskCountSpecificProject(projectid);
        if (alltask != null)
        {
            return Ok(alltask);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Task not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
}