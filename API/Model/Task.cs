namespace API.Model;

public class Task
{
    public int TaskId { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public int CreatedBy { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}