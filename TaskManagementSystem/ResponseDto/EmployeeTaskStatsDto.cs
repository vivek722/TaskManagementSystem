namespace TaskManagementSystem.ResponseDto;

public class EmployeeTaskStatsDto
{
    public string EmployeeName { get; set; }
    public int TotalTasks { get; set; }
}

public class EmployeeTaskAssinerDto
{
    public string AssinerName { get; set; }
    public string AssinerToName { get; set; }
    public string Task { get; set; }
}
