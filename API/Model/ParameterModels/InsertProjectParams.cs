namespace API.Model.ParameterModels;

public class InsertProjectParams (string name, string description, int createdBy)
{
    public string ProjectName { get; private set; } = name;
    public string Description { get; private set; } = description;
    public int CreatedBy { get; private set; } = createdBy;
}