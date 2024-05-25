using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsAllTasksForProjectDto : BaseDto
{
    public int ProjectId { get; set; }
}

public class ClientWantsAllTasksForProject(
    TaskRepository taskRepository) : BaseEventHandler<ClientWantsAllTasksForProjectDto>
{
    public override Task Handle(ClientWantsAllTasksForProjectDto dto, IWebSocketConnection socket)
    {
        IEnumerable<Model.Task> tasks = taskRepository.GetAllTasksForProject(dto.ProjectId);
        
        socket.SendDto(new ServerSendsAllTasksForProject
        {
            TasksList = tasks
        });

        return Task.CompletedTask;
    }
}