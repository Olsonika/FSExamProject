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
    private static readonly Dictionary<Guid, HashSet<int>> _clientToRooms = new();
    private static readonly Dictionary<int, HashSet<Guid>> _roomsToClients = new();

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
        if (_clientToRooms.TryGetValue(clientId, out var rooms))
        {
            foreach (var room in rooms)
            {
                _roomsToClients[room].Remove(clientId);
                if (_roomsToClients[room].Count == 0) _roomsToClients.Remove(room);
            }

            _clientToRooms.Remove(clientId);
        }

        _clients.Remove(clientId);
    }

    public static void JoinRoom(Guid clientId, int roomId)
    {
        if (!_roomsToClients.ContainsKey(roomId)) _roomsToClients[roomId] = new HashSet<Guid>();

        _roomsToClients[roomId].Add(clientId);

        if (!_clientToRooms.ContainsKey(clientId)) _clientToRooms[clientId] = new HashSet<int>();

        _clientToRooms[clientId].Add(roomId);
    }

   

    public static void BroadcastProject<T>(int roomId, T dto) where T : BaseDto
    {
        if (_roomsToClients.TryGetValue(roomId, out var clients))
            foreach (var clientId in clients)
                if (_clients.TryGetValue(clientId, out var connection))
                    connection.Connection.SendDto(dto);
    }
    

    public static List<IWebSocketConnection> GetClientsInRoom(int room)
    {
        return _roomsToClients.TryGetValue(room, out var clients)
            ? clients.Select(clientId => _clients[clientId].Connection).ToList()
            : new List<IWebSocketConnection>();
    }

    public static List<int> GetRoomsForClient(Guid clientId)
    {
        return _clientToRooms.TryGetValue(clientId, out var rooms)
            ? rooms.ToList()
            : new List<int>();
    }
    
    public static Dictionary<Guid, WsWithMetadata> GetAllClients()
    {
        return _clients;
    }
}