using System.Security.Authentication;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using Infrastructure.Model;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToGetProjectsDto : BaseDto
{
    
}

public class ClientWantsToGetProjects(ProjectRepository projectRepository) : BaseEventHandler<ClientWantsToGetProjectsDto>
{
    public override Task Handle(ClientWantsToGetProjectsDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
        if (client.IsAuthenticated)
        {
            IEnumerable<Project> projects = projectRepository.GetAllProjectsForUser(client.User.UserId);
            
            socket.SendDto(new ServerSendsProjects
            {
                ProjectsList = projects
            });
        }
        else
        {
            throw new AuthenticationException("User not authenticated!");
        }
        return Task.CompletedTask;
    }
}