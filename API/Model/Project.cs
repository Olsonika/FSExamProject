namespace API.Model;

public class Project
{
    public int ProjectId { get; set; }
    public string? ProjectName { get; set; } 
    public string? Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Email { get; set; }
}