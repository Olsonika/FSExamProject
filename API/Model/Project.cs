namespace Infrastructure.Model;

public class Project
{
    public int ProjectId { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}