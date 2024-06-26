using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Authentication;
using System.Text.Json;
using API.Helpers;
using API.Model.ServerEvents;
using API.Repositories;
using API.Security;
using Fleck;
using lib;
using Serilog;

public static class Startup
{
    public static void Main(string[] args)
    {
        var webApp = Start(args);
        webApp.Run();
    }
    
    public static WebApplication Start(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "\n{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}\n")
            .CreateLogger();
        Log.Information(JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<CredentialService>();
        builder.Services.AddSingleton<TokenService>();

       builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
          sourceBuilder => { sourceBuilder.EnableParameterLogging(); });
        builder.Services.AddSingleton<UserRepository>();
        builder.Services.AddSingleton<ProjectRepository>();
        builder.Services.AddSingleton<TaskRepository>();
        var services = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

   //     builder.WebHost.UseUrls("http://*:9999");
        var app = builder.Build();
  //      app.Services.GetService<ChatRepository>()!.ExecuteRebuildFromSqlScript();
   //     var port = Environment.GetEnvironmentVariable(ENV_VAR_KEYS.PORT.ToString()) ?? "8181";
        var server = new WebSocketServer("ws://0.0.0.0:8181");
        server.RestartAfterListenError = true;
        server.Start(socket =>
        {
            socket.OnOpen = () => WebSocketStateService.AddClient(socket.ConnectionInfo.Id, socket);
            socket.OnClose = () => WebSocketStateService.RemoveClient(socket.ConnectionInfo.Id);
            socket.OnMessage = async message =>
            {
                try
                {
                    await app.InvokeClientEventHandler(services, socket, message);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Global exception handler");
                    if (app.Environment.IsProduction() && (e is ValidationException || e is AuthenticationException))
                    {
                        socket.SendDto(new ServerSendsErrorMessageToClient()
                        {
                            ErrorMessage = "Something went wrong",
                            ReceivedMessage = message
                        });
                    }
                    else
                    {
                        socket.SendDto(new ServerSendsErrorMessageToClient
                            { ErrorMessage = e.Message, ReceivedMessage = message });
                    }
                }
            };
        });
        return app;
    }
}

