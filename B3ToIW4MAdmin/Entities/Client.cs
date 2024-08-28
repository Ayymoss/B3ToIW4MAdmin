﻿namespace B3ToIW4MAdmin.Entities;

public class Client
{
    public long Id { get; set; }
    public string Ip { get; set; } = null!;
    public long Connections { get; set; }
    public string? Guid { get; set; }
    public string Name { get; set; } = null!;
    public string TimeAdd { get; set; } = null!;
    public long TimeEdit { get; set; }
}
