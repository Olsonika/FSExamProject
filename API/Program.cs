using System.Reflection;
using Fleck;
using Infrastructure;
using lib;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());

var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseCors(options =>
    options
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
);

var server = new WebSocketServer("ws://0.0.0.0:8181");

server.Start(socket =>
{
    socket.OnOpen = () => { StateService.AddConnection(socket.ConnectionInfo.Id, socket); };

    socket.OnMessage = async message =>
    {
        try
        {
            await app.InvokeClientEventHandler(clientEventHandlers, socket, message);
        }
        catch (Exception e)
        {
            
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
            e.Handle(socket, message);
        }
    };
});


Console.ReadLine();

