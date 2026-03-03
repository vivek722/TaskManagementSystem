using TaskManagementSystem.Model;

namespace TaskManagementSystem.DtoModels;

public class EmployeeDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Role role { get; set; } 
}
