namespace TaskManagementSystem.Model;

public class BaseClass
{
    public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    public bool status { get; set; }
}
