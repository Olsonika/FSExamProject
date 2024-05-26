using lib;

namespace API.Model.ServerEvents;

public class ServerInsertsTask : BaseDto
{
    public Task task { set; get; }
}