using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.services;

namespace TaskManagementSystem.Controllers;

[ApiController]
public class TaskController : Controller
{
    private readonly ITaskServices taskServices;
    public TaskController(ITaskServices taskServices)
    {
        this.taskServices = taskServices;
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

    [HttpPost("api/AddTasks")]
    public async Task<IActionResult> AddTasks(TaskDto taskDto)
    {
        if(taskDto != null)
        {
            var task = new TaskManage
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                Assignerid = taskDto.Assignerid,
                assignToId = taskDto.assignToid,
                CreatedAt = DateTime.Now,
                dueDate = taskDto.dueDate,
                employeeId = taskDto.employeeId,
                Proirity = taskDto.Proirity,
                StartDate = taskDto.StartDate,
                EndDate = taskDto.EndDate,
                projectId = taskDto.projectId,

            };
            //foreach (var stask in taskDto.subTaskManegs) {
            //    var subtask = new SubTaskManeg
            //    {

            //        Name = stask.Name,
            //        Description = stask.Description,
            //        Assigner = stask.Assigner,
            //        assignTo = stask.assignTo,
            //        dueDate = stask.dueDate,
            //        Proirity = stask.Proirity,
            //        StartDate = stask.StartDate,
            //        EndDate = stask.EndDate,

            //    };
            //}


            var result = await taskServices.AddTask(task);
            return Ok(result);
        }
        return BadRequest("Invalid Project Data");
    }
}
