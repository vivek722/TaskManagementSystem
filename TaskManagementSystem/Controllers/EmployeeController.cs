using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Controllers;

[ApiController]
public class EmployeeController : Controller
{
    private readonly IEmployeeServices _employeeServices;
    public EmployeeController(IEmployeeServices employeeServices)
    {
        _employeeServices = employeeServices;
    }
    [HttpGet("api/Getemployees")]
    public async Task<IActionResult> GetAllEmployees()
    {

        var employees = await _employeeServices.GetAllEmployees();
        if (employees != null)
        {
            return Ok(employees);
        }
        return NotFound("No Employees Found");
    }

    [HttpPost("api/Addemployees")]
    public async Task<IActionResult> AddEmployees([FromBody] EmployeeDto employeeDto)
    {
        if (employeeDto != null)
        {
            var employee = new EmployeeModel
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                role = employeeDto.role,
                status = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            }; 
            var result = await _employeeServices.AddEmployee(employee);
            return Ok(result);
        }
        return BadRequest("Invalid Employee Data");
    }
}
