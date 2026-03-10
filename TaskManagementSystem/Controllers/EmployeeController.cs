using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManagementSystem.DataResponseModel;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.Utility.CacheService;

namespace TaskManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("fixed")]
public class EmployeeController : Controller
{
    private readonly IEmployeeServices _employeeServices;
    private readonly ICacheService _cacheService;
    private readonly IMapper mapper;
    public EmployeeController(IEmployeeServices employeeServices, ICacheService cacheService,IMapper mapper)
    {
        _employeeServices = employeeServices;
        _cacheService = cacheService;
        this.mapper = mapper;
    }
    [HttpGet("Getemployees")]
    public async Task<IActionResult> GetAllEmployees()
    {
        string cacheKey = "EmployeesCache";

        var cachedEmployees = await _cacheService.GetAsync<List<EmployeeDto>>(cacheKey);
        if (cachedEmployees != null)
        {
           
            return Ok(new DataSuccessResponseModel<List<EmployeeDto>>() { data = cachedEmployees, Message = "GetAllEmployees successfully", StatusCode = StatusCodes.Status200OK });
        }

        var employees = await _employeeServices.GetAllEmployees();
        if (employees != null)
        {
            await _cacheService.SetAsync(cacheKey, employees);
            return Ok(new DataSuccessResponseModel<List<EmployeeModel>>() { data = employees, Message = "GetAllEmployees successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Employee not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    //[Authorize(Roles = "Admin")]
    [HttpPost("Addemployees")]
    public async Task<IActionResult> AddEmployees([FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto != null)
        {
            var employee = mapper.Map<EmployeeModel>(employeeDto);
            employee.password = BCrypt.Net.BCrypt.HashPassword(employeeDto.password);

            employee.status = true;
            employee.CreatedAt = DateTime.Now;
            employee.UpdatedAt = null;
           
            var result = await _employeeServices.AddEmployee(employee);
            return Ok(new DataSuccessResponseModel<EmployeeModel>() { data = employee, Message = "AddEmployees successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "invalid Employee Data",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteEmployee/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var employees = await _employeeServices.DeleteEmployee(id);
            if (employees == true)
            {
                return Ok(employees);
            }
        }
       catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Employee not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("GetemployeesWithProjectAndTask")]
    public async Task<IActionResult> GetEmployeesWithProjectAndTask()
    {
        var employees = await _employeeServices.GetAllEmployeesWithProjectAndTask();
        if (employees != null)
        {
            return Ok(new DataSuccessResponseModel<List<EmployeeModel>>() { data = employees, Message = "GetEmployeesWithProjectAndTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound("No Employees Found");
    }
    [HttpGet("GetSpecificemployeesWithProjectAndTask")]
    public async Task<IActionResult> GetSpecificemployeesWithProjectAndTask(int id)
    {
       
            var employee = await _employeeServices.GetByIdEmployeeWithProjectAndTask(id);
            if (employee != null)
            {
                return Ok(new DataSuccessResponseModel<EmployeeModel>() { data = employee, Message = "GetByIdEmployeeWithProjectAndTask successfully", StatusCode = StatusCodes.Status200OK });
            }
            return NotFound(new DataErrorResponseModel
            {
                Message = "Employee not found",
                StatusCode = StatusCodes.Status404NotFound
            });
        }

    [HttpGet("GetEmployeesWhichHaveNoTask")]
    public async Task<IActionResult> GetEmployeesWhichHaveNoTask()
    {
        var employee = await _employeeServices.GetEmployeeWhichHaveNotanyTask();
        if (employee != null)
        {
            return Ok(new DataSuccessResponseModel<List<EmployeeModel>>() { data = employee, Message = "GetEmployeesWhichHaveNoTask successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Employee not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [HttpGet("getEmployeeById")]
    public async Task<IActionResult> getEmployeeById(int id)
    {
        var employee = await _employeeServices.GetByIdEmployee(id);
        if (employee != null)
        {
            return Ok(new DataSuccessResponseModel<EmployeeModel>() { data = employee, Message = "GetEmployeeById successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Employee not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getoverduetaskforemployee/{id}")]
    public async Task<IActionResult> getOverDueTaskForEmployee(int id)
    {
        var employee = await _employeeServices.EmployeeWithMostOverDueTask(id);
        if (employee != null)
        {
            return Ok(new DataSuccessResponseModel<EmployeeModel>() { data = employee, Message = "getOverDueTaskForEmployee successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Employee not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }

    [HttpGet("getcompletedtaskforemployee/{id}")]
    public async Task<IActionResult> getCompletedTaskForEmployee(int id)
    {
        var employee = await _employeeServices.EmployeeWithMostCompletedTask(id);
        if (employee != null)
        {
            return Ok(new DataSuccessResponseModel<EmployeeModel>() { data = employee, Message = "getCompletedTaskForEmployee successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "Employee not found",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateEmployee/{id}")]
    public async Task<IActionResult> UpdateEmployees([FromRoute] int id, [FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto != null)
        {
            var employee = mapper.Map<EmployeeModel>(employeeDto);

            employee.status = true;
            employee.CreatedAt = employee.CreatedAt;
            employee.UpdatedAt = DateTime.Now;
           

            var result = await _employeeServices.UpdateEmployee(id, employee);

            return Ok(new DataSuccessResponseModel<EmployeeModel>() { data = employee, Message = "UpdateEmployees successfully", StatusCode = StatusCodes.Status200OK });
        }
        return NotFound(new DataErrorResponseModel
        {
            Message = "bad request",
            StatusCode = StatusCodes.Status404NotFound
        });
    }
}
