using TaskManagementSystem.Model;

namespace TaskManagementSystem.DtoModels;

public class TaskDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Assignerid { get; set; }

    public int assignToid { get; set; }
    public DateTime dueDate { get; set; }
    public Proirity Proirity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? employeeId { get; set; }
    public int projectId { get; set; }
}

public class SubTaskManegdto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Assignerid { get; set; }

    public int assignToid { get; set; }
    public DateTime dueDate { get; set; }
    public Proirity Proirity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TaskManageid { get; set; }
}