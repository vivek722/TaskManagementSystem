namespace TaskManagementSystem.Model;

public class projectModel : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int employeeId { get; set; }
    public EmployeeModel employeeModel { get; set; }

    public virtual ICollection<TaskManage> TaskManages { get; set; } = new List<TaskManage>();
}
