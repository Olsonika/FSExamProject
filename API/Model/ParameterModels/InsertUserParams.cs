namespace API.Model.ParameterModels;

public class InsertUserParams(string email, string hash, string salt)
{
    public string Email { get; private set; } = email;
    public string Hash { get; private set; } = hash;
    public string Salt { get; private set; } = salt;
}