using lib;

namespace API.Model.ServerEvents;

public class ServerSendsAllTasksForProject : BaseDto
{
    public IEnumerable<Task> TasksList { get; set; }
}