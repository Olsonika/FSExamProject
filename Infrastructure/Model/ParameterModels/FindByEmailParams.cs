namespace Infrastructure.Model.ParameterModels;

public class FindByEmailParams(string email)
{
    public string email { get; private set; } = email;
}