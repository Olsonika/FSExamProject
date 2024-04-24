using lib;

namespace Api.Model.ServerEvents;

public class ServerAuthenticatesUser : BaseDto
{
    public string? jwt { get; set; }
}