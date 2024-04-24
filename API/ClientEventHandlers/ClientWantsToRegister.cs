using System.ComponentModel.DataAnnotations;
using Api.Model.ServerEvents;
using Fleck;
using Infrastructure.Model.ParameterModels;
using lib;

namespace API.ClientEventHandlers;

public class ClientWantsToRegisterDto : BaseDto
{
    [EmailAddress] public string Email { get; set; }

    [MinLength(6)] public string Password { get; set; }
}

[ValidateDataAnnotations]
public class ClientWantsToRegister(
    UserRepository userRepository,
    CredentialService credentialService,
    TokenService tokenService
) : BaseEventHandler<ClientWantsToRegisterDto>
{
    public override Task Handle(ClientWantsToRegisterDto dto, IWebSocketConnection socket)
    {
        if (userRepository.DoesUserAlreadyExist(new FindByEmailParams(dto.Email)))
            throw new ValidationException("User with this email already exists");
        var salt = credentialService.GenerateSalt();
        var hash = credentialService.Hash(dto.Password, salt);
        var user = userRepository.InsertUser(new InsertUserParams(dto.Email, hash, salt));
        var token = tokenService.IssueJwt(user);
        WebSocketStateService.GetClient(socket.ConnectionInfo.Id).IsAuthenticated = true;
        WebSocketStateService.GetClient(socket.ConnectionInfo.Id).User = user;
        socket.SendDto(new ServerAuthenticatesUser { jwt = token });
        return Task.CompletedTask;
    }
}