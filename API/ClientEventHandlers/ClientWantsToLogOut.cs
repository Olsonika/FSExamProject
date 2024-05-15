using System.Security.Authentication;
using API.Model.ServerEvents;
using Fleck;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToLogOutDto : BaseDto
{
    
}

public class ClientWantsToLogOut() : BaseEventHandler<ClientWantsToLogOutDto>
{
    public override Task Handle(ClientWantsToLogOutDto dto, IWebSocketConnection socket)
    {
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
        
        if (client.IsAuthenticated)
        {
            client.IsAuthenticated = false;
            client.User = null; 
            socket.SendDto(new ServerLogsOutUser());
        }
        else
        {
            throw new AuthenticationException("User not authenticated!");
        }

        return Task.CompletedTask;
    }
}
