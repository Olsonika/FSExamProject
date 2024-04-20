using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;
using Serilog;

public static class GlobalExceptionHandler
{
    public static void Handle(this Exception exception, IWebSocketConnection socket, string? message)
    {
        Log.Error(exception, "Global exception handler");

        if (exception is ValidationException validationException)
        {
            var errorMessage = new ServerSendsErrorMessageToClient
            {
                ErrorMessage = validationException.Message
            };

            var serializedError = JsonSerializer.Serialize(errorMessage);
            socket.Send(serializedError);
        }
        else
        {
            var errorMessage = new ServerSendsErrorMessageToClient
            {
                ErrorMessage = "An error occurred"
            };

            var serializedError = JsonSerializer.Serialize(errorMessage);
            socket.Send(serializedError);
        }
    }
}

public class ServerSendsErrorMessageToClient : BaseDto
{
    public string ErrorMessage { get; set; }
}