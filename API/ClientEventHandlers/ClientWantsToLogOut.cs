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
        // Retrieve the client's authentication status
        var client = WebSocketStateService.GetClient(socket.ConnectionInfo.Id);
    
        // Check if the client is authenticated
        if (client.IsAuthenticated)
        {
            // Invalidate authentication status
            client.IsAuthenticated = false;
        
            // Clear user information
            client.User = null; // Or any other appropriate action to clear user-related information
        
            // Additional cleanup if necessary
        
            // Inform the client that logout was successful
            socket.SendDto(new ServerLogsOutUser());
        }
        else
        {
            // Inform the client that they are not authenticated
            throw new AuthenticationException("User not authenticated!");
        }

        return Task.CompletedTask;
    }
}
