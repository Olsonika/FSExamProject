using System.Security.Authentication;
using Fleck;
using lib;

public class RequireAuthenticationAttribute : BaseEventFilter
{
    public override Task Handle<T>(IWebSocketConnection socket, T dto)
    {
        if (!WebSocketStateService.GetClient(socket.ConnectionInfo.Id).IsAuthenticated)
            throw new AuthenticationException("Client is not authenticated!");
        return Task.CompletedTask;
    }
}