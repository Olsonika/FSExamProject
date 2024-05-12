﻿using lib;

namespace API.Model.ServerEvents;

public class ServerInsertsProject : BaseDto
{
    public int ProjectId { get; set; }
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}