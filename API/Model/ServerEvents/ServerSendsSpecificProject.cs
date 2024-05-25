using lib;

namespace API.Model.ServerEvents;

public class ServerSendsSpecificProject : BaseDto
{
    public Project Project { get; set; }
}