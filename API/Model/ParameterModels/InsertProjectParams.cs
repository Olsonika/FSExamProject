namespace API.Model.ParameterModels;

public class InsertProjectParams (string name, string description, int createdBy)
{
    public string ProjectName { get; set; } = name;
    public string Description { get; set; } = description;
    public int CreatedBy { get; set; } = createdBy;
}