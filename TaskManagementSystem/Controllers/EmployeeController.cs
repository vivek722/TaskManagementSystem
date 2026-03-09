using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    public EmployeeController(IEmployeeServices employeeServices, ICacheService cacheService)
    {
        _employeeServices = employeeServices;
        _cacheService = cacheService;
    }
    [HttpGet("api/Getemployees")]
    public async Task<IActionResult> GetAllEmployees()
    {
        string cacheKey = "EmployeesCache";

        var cachedEmployees = await _cacheService.GetAsync<List<EmployeeDto>>(cacheKey);
        if (cachedEmployees != null)
        {
            return Ok(cachedEmployees);
        }

        var employees = await _employeeServices.GetAllEmployees();
        if (employees != null)
        {
            await _cacheService.SetAsync(cacheKey, employees);
            return Ok(employees);
        }
        return NotFound("No Employees Found");
    }
    //[Authorize(Roles = "Admin")]
    [HttpPost("/Addemployees")]
    public async Task<IActionResult> AddEmployees([FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto != null)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(employeeDto.password);

            var employee = new EmployeeModel
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                role = employeeDto.role,
                password = password,
                status = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
            var result = await _employeeServices.AddEmployee(employee);
            return Ok(result);
        }
        return BadRequest("Invalid Employee Data");
    }
    [HttpGet("/GetemployeesWithProjectAndTask")]
    public async Task<IActionResult> GetEmployeesWithProjectAndTask()
    {
        var employees = await _employeeServices.GetAllEmployeesWithProjectAndTask();
        if (employees != null)
        {
            return Ok(employees);
        }
        return NotFound("No Employees Found");
    }
    [HttpGet("/GetSpecificemployeesWithProjectAndTask")]
    public async Task<IActionResult> GetSpecificemployeesWithProjectAndTask(int id)
    {
        var employee = await _employeeServices.GetByIdEmployeeWithProjectAndTask(id);
        if (employee != null)
        {
            return Ok(employee);
        }
        return NotFound("No Employees Found");
    }
    [HttpGet("/getEmployeeById")]
    public async Task<IActionResult> getEmployeeById(int id)
    {
        var employee = await _employeeServices.GetByIdEmployee(id);
        if (employee != null)
        {
            return Ok(employee);
        }
        return NotFound("No Employees Found");
    }

    [HttpPut("UpdateEmployee/{id}")]
    public async Task<IActionResult> UpdateEmployees([FromRoute] int id, [FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto != null)
        {
            var employee = new EmployeeModel
            {
                Id = id,
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                role = employeeDto.role,
                status = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            var result = await _employeeServices.UpdateEmployee(id, employee);

            return Ok(result);
        }

        return BadRequest("Invalid Employee Data");
    }
}
