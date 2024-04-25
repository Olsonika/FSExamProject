using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using API.Model.ParameterModels;
using Api.Models.ServerEvents;
using API.Repositories;
using Fleck;
using lib;

public class ClientWantsToAuthenticateWithJwtDto : BaseDto
{
    [Required] public string? jwt { get; set; }
}

/*[ValidateDataAnnotations]
public class ClientWantsToAuthenticateWithJwt(
        UserRepository userRepository,
        TokenService tokenService)
    : BaseEventHandler<ClientWantsToAuthenticateWithJwtDto>
{
    public override async Task Handle(ClientWantsToAuthenticateWithJwtDto dto, IWebSocketConnection socket)
    {
        var claims = tokenService.ValidateJwtAndReturnClaims(dto.jwt!);
        var user = userRepository.GetUser(new FindByEmailParams(claims["email"]));
        WebSocketStateService.GetClient(socket.ConnectionInfo.Id).User = user;
        WebSocketStateService.GetClient(socket.ConnectionInfo.Id).IsAuthenticated = true;
        socket.SendDto(new ServerAuthenticatesUserFromJwt());
    }
}*/