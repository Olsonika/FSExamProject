namespace API.Model.ParameterModels;

public class InsertTaskParams(string taskName, string description, DateTime duedate, int projectId, int createdBy)
{
    public string TaskName { get; set; } = taskName;
    public string Description { get; set; } = description;
    public DateTime DueDate { get; set; } = duedate;
    public int ProjectId { get; set; } = projectId;
    public int CreatedBy { get; set; } = createdBy;
}