using lib;

namespace API.Model.ServerEvents;

public class ServerInsertsProject : BaseDto
{
    public Project project { get; set; }
}