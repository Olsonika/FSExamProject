using API.Model.ParameterModels;
using API.Model.ServerEvents;
using API.Repositories;
using Fleck;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToCreateTaskDto : BaseDto
{
    public string TaskName { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int ProjectId { get; set; }
}

public class ClientWantsToCreateTask(
    TaskRepository taskRepository) : BaseEventHandler<ClientWantsToCreateTaskDto>
{
    public override Task Handle(ClientWantsToCreateTaskDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
        
        // if (client.IsAuthenticated)
        // {
        var insertTaskParams = new InsertTaskParams(dto.TaskName, dto.Description, dto.DueDate, dto.ProjectId, client.User.UserId);
        var task = taskRepository.InsertTask(insertTaskParams);

        var taskWithInfo = new Model.Task
        {
            TaskId = task.TaskId,
            TaskName = task.TaskName,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            CreatedBy = task.CreatedBy,
            ProjectId = task.ProjectId,
            CreatedAt = task.CreatedAt
        };
        
        foreach (var connectedClient in WebSocketStateService.GetAllClients())
        {
            if (connectedClient.Value.IsAuthenticated)
            {
                connectedClient.Value.Connection.SendDto(new ServerInsertsTask
                {
                    task = taskWithInfo
                });
            }
        }
        // }
        // else
        // {
        //     throw new AuthenticationException("User not authenticated!");
        //}

        return Task.CompletedTask;
    }
}