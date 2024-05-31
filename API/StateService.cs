using Fleck;
using Infrastructure.Model;
using lib;

public class WsWithMetadata
{
    public IWebSocketConnection Connection { get; }
    public bool IsAuthenticated { get; set; } = false;
    public EndUser? User { get; set; }

    // Constructor
    public WsWithMetadata(IWebSocketConnection connection)
    {
        Connection = connection;
    }
}

public static class WebSocketStateService
{
    private static readonly Dictionary<Guid, WsWithMetadata> _clients = new();

    public static WsWithMetadata GetClient(Guid clientId)
    {
        return _clients[clientId];
    }

    public static void AddClient(Guid clientId, IWebSocketConnection connection)
    {
        _clients.TryAdd(clientId, new WsWithMetadata(connection));
    }

    public static void RemoveClient(Guid clientId)
    {
        _clients.Remove(clientId);
    }
    
    
    public static Dictionary<Guid, WsWithMetadata> GetAllClients()
    {
        return _clients;
    }
}