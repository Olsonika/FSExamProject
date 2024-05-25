using lib;

namespace API.Model.ServerEvents;

public class ServerDeletesProject : BaseDto
{
    public int ProjectId { get; set; }
}