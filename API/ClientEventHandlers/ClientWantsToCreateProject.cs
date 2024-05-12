using System.Security.Authentication;
using API.Model.ParameterModels;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToCreateProjectDto : BaseDto
{
    public string ProjectName { get; set; }
    public string Description { get; set; }
}

public class ClientWantsToCreateProject(
    ProjectRepository projectRepository,
    UsersInProjectRepository usersInProjectRepository) : BaseEventHandler<ClientWantsToCreateProjectDto>
{
    public override Task Handle(ClientWantsToCreateProjectDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
        if (client.IsAuthenticated)
        {
            var insertProjectParams = new InsertProjectParams(dto.ProjectName, dto.Description, client.User.UserId);
            var project = projectRepository.InsertProject(insertProjectParams);
            usersInProjectRepository.InsertUsersInProject(client.User.UserId, project.ProjectId);
            socket.SendDto(new ServerInsertsProject
            {
                ProjectId = project.ProjectId,
                Name = project.ProjectName,
                CreatedAt = project.CreatedAt,
                CreatedBy = project.CreatedBy,
                Description = project.Description,
            });
        }
        else
        {
            throw new AuthenticationException("User not authenticated!");
        }

        return Task.CompletedTask;
    }
}