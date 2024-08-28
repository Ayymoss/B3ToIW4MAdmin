namespace B3ToIW4MAdmin.Entities;

public class Penalty
{
    public long Id { get; set; }
    public string Type { get; set; } = null!;
    public long ClientId { get; set; }
    public long AdminId { get; set; }
    public string Reason { get; set; } = null!;
    public long TimeAdd { get; set; }
    public long TimeExpire { get; set; }
}
