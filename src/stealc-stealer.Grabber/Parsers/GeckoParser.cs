using System.Security.Cryptography;
using System.Text;

namespace Stealc.Grabber.Parsers;

public sealed class GeckoParser
{
    // Firefox stores logins in logins.json / key4.db (NSS)
    // Cookies in cookies.sqlite (moz_cookies table)
    // cert9.db contains certificates

    public List<GeckoLogin> ParseLogins(string profilePath)
    {
        // logins.json fields: hostname, encryptedUsername, encryptedPassword, guid
        return
        [
            SimulatedLogin("https://example.com", "user@example.com"),
            SimulatedLogin("https://webmail.example.org", "admin"),
        ];
    }

    public List<GeckoCookie> ParseCookies(string profilePath)
    {
        // moz_cookies: name, value, host, path, expiry, isSecure, isHttpOnly, sameSite
        return
        [
            new() { Host = ".example.com", Name = "PHPSESSID", Secure = true },
            new() { Host = ".github.com", Name = "logged_in", Secure = true },
        ];
    }

    public byte[] DecryptNss(byte[] encrypted, byte[] masterPassword)
    {
        return SHA256.HashData(encrypted.Concat(masterPassword).ToArray());
    }

    private static GeckoLogin SimulatedLogin(string url, string user)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(url + user));
        return new GeckoLogin
        {
            Hostname = url, Username = user,
            PasswordHash = Convert.ToHexString(hash[..8]).ToLowerInvariant(),
            Guid = Guid.NewGuid().ToString("N")[..12]
        };
    }
}

public sealed class GeckoLogin
{
    public required string Hostname { get; init; }
    public required string Username { get; init; }
    public string PasswordHash { get; init; } = "";
    public string Guid { get; init; } = "";
}

public sealed class GeckoCookie
{
    public required string Host { get; init; }
    public required string Name { get; init; }
    public bool Secure { get; init; }
}
