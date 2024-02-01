namespace B3ToIW4MAdmin.Entities;

public class Client
{
    public long Id { get; set; }
    public string Ip { get; set; } = null!;
    public long Connections { get; set; }
    public string? Guid { get; set; }
    public string Pbid { get; set; } = null!;
    public string Name { get; set; } = null!;
    public long AutoLogin { get; set; }
    public long MaskLevel { get; set; }
    public long GroupBits { get; set; }
    public string Greeting { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string TimeAdd { get; set; } = null!;
    public long TimeEdit { get; set; }
    public long Show { get; set; }
    public long Spammer { get; set; }
    public long OnlyWithClient { get; set; }
    public long TrgAdmin { get; set; }
    public long ChatMute { get; set; }
    public long Regular { get; set; }
}
