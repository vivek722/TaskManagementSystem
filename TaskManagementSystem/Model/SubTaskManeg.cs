namespace TaskManagementSystem.Model;

public class SubTaskManeg : BaseClass
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Assignerid { get; set; }
     

    public int assignToid { get; set; }
    public DateTime dueDate { get; set; }
    public Proirity Proirity { get; set; }
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int TaskManageid  { get; set; }
    public TaskManage TaskManage { get; set; }
}
