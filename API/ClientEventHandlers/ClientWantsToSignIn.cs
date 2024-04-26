using System.Security.Authentication;
using API.Model.ParameterModels;
using Api.Model.ServerEvents;
using API.Repositories;
using API.Security;
using Fleck;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToSignInDto : BaseDto
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class ClientWantsToAuthenticate(
        UserRepository userRepository,
        TokenService tokenService,
        CredentialService credentialService)
    : BaseEventHandler<ClientWantsToSignInDto>
{
    public override Task Handle(ClientWantsToSignInDto request, IWebSocketConnection socket)
    {

        var user = userRepository.GetUser(new FindByEmailParams(request.Email!));
        var expectedHash = credentialService.Hash(request.Password!, user.Salt!);
        if (!expectedHash.Equals(user.Hash)) throw new AuthenticationException("Wrong credentials!");
        WebSocketStateService.GetClient(socket.ConnectionInfo.Id).IsAuthenticated = true;
        WebSocketStateService.GetClient(socket.ConnectionInfo.Id).User = user;
        socket.SendDto(new ServerAuthenticatesUser { jwt = tokenService.IssueJwt(user) });
        return Task.CompletedTask;
    }
}