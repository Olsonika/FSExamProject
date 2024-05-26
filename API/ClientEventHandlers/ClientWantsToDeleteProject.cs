using System.Security.Authentication;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToDeleteProjectDto : BaseDto
{
    public int ProjectId { get; set; }
}

public class ClientWantsToDeleteProject(
    ProjectRepository projectRepository) : BaseEventHandler<ClientWantsToDeleteProjectDto>
{
    public override Task Handle(ClientWantsToDeleteProjectDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
        /*if (client.IsAuthenticated)
        {*/
            projectRepository.DeleteProject(dto.ProjectId);
            
            foreach (var connectedClient in WebSocketStateService.GetAllClients())
            {
                //if (connectedClient.Value.IsAuthenticated)
               // {
                    connectedClient.Value.Connection.SendDto(new ServerDeletesProject
                    {
                        ProjectId = dto.ProjectId
                    });
               // }
            }
       /* }
        else
        {
            throw new AuthenticationException("User not authenticated!");
        }*/
        return Task.CompletedTask;
    }
}