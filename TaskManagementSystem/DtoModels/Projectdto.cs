namespace TaskManagementSystem.DtoModels;

public class Projectdto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int employeeId { get; set; }
}
