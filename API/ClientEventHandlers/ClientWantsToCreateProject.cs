using System.Security.Authentication;
using API.Model;
using API.Model.ParameterModels;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using lib;
using Task = System.Threading.Tasks.Task;

namespace API.ClientEventHandlers;

public class ClientWantsToCreateProjectDto : BaseDto
{
    public string ProjectName { get; set; }
    public string Description { get; set; }
}

public class ClientWantsToCreateProject(
    ProjectRepository projectRepository) : BaseEventHandler<ClientWantsToCreateProjectDto>
{
    public override Task Handle(ClientWantsToCreateProjectDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
        if (client.IsAuthenticated)
        {
            var insertProjectParams = new InsertProjectParams(dto.ProjectName, dto.Description, client.User.UserId);
            var project = projectRepository.InsertProject(insertProjectParams);
            
            var projectWithInfo = new Project
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                CreatedAt = project.CreatedAt,
                CreatedBy = project.CreatedBy,
                Description = project.Description,
                Email = project.Email
            };
            
            foreach (var connectedClient in WebSocketStateService.GetAllClients())
            {
                if (connectedClient.Value.IsAuthenticated)
                {
                    connectedClient.Value.Connection.SendDto(new ServerInsertsProject
                    {
                        project = projectWithInfo
                    });
                }
            }
        }
        else
        {
            throw new AuthenticationException("User not authenticated!");
       }

        return Task.CompletedTask;
    }
}