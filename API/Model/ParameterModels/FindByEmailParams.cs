namespace API.Model.ParameterModels;

public class FindByEmailParams(string email)
{
    public string Email { get; private set; } = email;
}