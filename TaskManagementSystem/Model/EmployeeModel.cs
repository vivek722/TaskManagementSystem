namespace TaskManagementSystem.Model;

public class EmployeeModel : BaseClass
{
    public string Name { get; set; }
    public string Email { get; set; }
    public Role role { get; set; }

    //public int? TaskManageid { get; set; }
    //public TaskManage TaskManage { get; set; }

    //public int? ProjectId { get; set; }
    //public projectModel ProjectModel { get; set; }
    public virtual ICollection<projectModel> ProjectModel { get; set; } = new List<projectModel>();
    public virtual ICollection<TaskManage> TaskManage { get; set; } = new List<TaskManage>();

}

public enum Role
{
    Admin,
    Employee
}
