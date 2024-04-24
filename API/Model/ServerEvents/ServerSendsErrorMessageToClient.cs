using lib;

namespace API.Model.ServerEvents;

public class ServerSendsErrorMessageToClient : BaseDto
{
    public string? ErrorMessage { get; set; }
    public string? ReceivedMessage { get; set; }
}