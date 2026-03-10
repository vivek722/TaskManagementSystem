namespace TaskManagementSystem.DtoModels;

public class projectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int ProjectManagerId { get; set; }
}
