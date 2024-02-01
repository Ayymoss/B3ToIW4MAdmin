namespace B3ToIW4MAdmin.Entities;

public class Penalty
{
    public long Id { get; set; }
    public string Type { get; set; } = null!;
    public long ClientId { get; set; }
    public long AdminId { get; set; }
    public long Duration { get; set; }
    public long Inactive { get; set; }
    public string Keyword { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public string Data { get; set; } = null!;
    public long TimeAdd { get; set; }
    public long TimeEdit { get; set; }
    public long TimeExpire { get; set; }
    public string Server { get; set; } = null!;
    public long InactiveAdminId { get; set; }
    public long TimeInactive { get; set; }
    public long EditAdminId { get; set; }
    public long EditTime { get; set; }
    public string EditReason { get; set; } = null!;
}
