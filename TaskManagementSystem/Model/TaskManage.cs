namespace TaskManagementSystem.Model;

public class TaskManage : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Assignerid { get; set; }

    public int  assignToId { get; set; }
    public DateTime dueDate { get; set; }
    public Proirity Proirity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public status taskStatus { get; set; }
    public int? employeeId { get; set; }

    public EmployeeModel employeeModel { get; set; }

    public int projectId { get; set; }
    public projectModel ProjectModel { get; set; }
    public virtual ICollection<SubTaskManeg> SubTasks { get; set; } = new List<SubTaskManeg>();
}
public enum Proirity
{
    high,
    medidum,
    low
}
public enum status
{
    Pending,
    InProgress,
    Completed,
    Blocked
}