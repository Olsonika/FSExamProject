namespace Infrastructure.Model;

public class EndUser
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? Hash { get; set; }
    public string? Salt { get; set; }
    public bool Isadmin { get; set; }
}