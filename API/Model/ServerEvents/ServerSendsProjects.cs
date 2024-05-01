using Infrastructure.Model;
using lib;

namespace API.Model.ServerEvents;

public class ServerSendsProjects : BaseDto
{
    public IEnumerable<Project> ProjectsList { get; set; }
}