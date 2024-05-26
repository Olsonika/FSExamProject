using System.Security.Authentication;
using API.Model;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using Infrastructure.Model;
using lib;
using Task = System.Threading.Tasks.Task;

namespace API.ClientEventHandlers;

public class ClientWantsToGetProjectsDto : BaseDto
{
    
}

public class ClientWantsToGetProjects(ProjectRepository projectRepository) : BaseEventHandler<ClientWantsToGetProjectsDto>
{
    public override Task Handle(ClientWantsToGetProjectsDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
       // if (client.IsAuthenticated)
       // {
            IEnumerable<Project> projects = projectRepository.GetAllProjects();
            
            socket.SendDto(new ServerSendsProjects
            {
                ProjectsList = projects
            });
      //  }
       // else
      //  {
      //      throw new AuthenticationException("User not authenticated!");
      //  }
        return Task.CompletedTask;
    }
}