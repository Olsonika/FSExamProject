using API.Model;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using lib;
using Task = System.Threading.Tasks.Task;

namespace API.ClientEventHandlers;

public class ClientWantsToGetProjectByIdDto : BaseDto
{
    public int ProjectId { get; set; }
}

public class ClientWantsToGetProjectById(
    ProjectRepository projectRepository) : BaseEventHandler<ClientWantsToGetProjectByIdDto>
{
    public override Task Handle(ClientWantsToGetProjectByIdDto dto, IWebSocketConnection socket)
    {
        Project project = projectRepository.GetProjectById(dto.ProjectId);
        
        socket.SendDto(new ServerSendsSpecificProject
        {
            Project = project
        });
        
        return Task.CompletedTask;
    }
}