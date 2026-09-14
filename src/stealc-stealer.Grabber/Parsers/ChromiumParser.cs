using System.Security.Cryptography;
using System.Text;

namespace Stealc.Grabber.Parsers;

public sealed class ChromiumParser
{
    // Chromium Login Data table: origin_url, action_url, username_element,
    //   username_value, password_element, password_value, signon_realm, date_created
    // Cookies table: host_key, name, encrypted_value, path, expires_utc, is_httponly
    // Web Data (autofill): name, value, count, date_created
    // Web Data (credit cards): name_on_card, card_number_encrypted, expiration_month/year

    public List<ChromiumLogin> ParseLogins(string profilePath)
    {
        var dbPath = Path.Combine(profilePath, "Default", "Login Data");
        // In lab mode, return simulated entries
        return
        [
            Simulated("https://example.com", "user@example.com"),
            Simulated("https://mail.example.org", "admin"),
        ];
    }

    public List<ChromiumCookie> ParseCookies(string profilePath)
    {
        var dbPath = Path.Combine(profilePath, "Default", "Cookies");
        return
        [
            new() { Host = ".example.com", Name = "session_id", Encrypted = true, HttpOnly = true },
            new() { Host = ".github.com", Name = "__Host-session", Encrypted = true, HttpOnly = true },
        ];
    }

    public List<string> ParseBookmarks(string profilePath)
    {
        var file = Path.Combine(profilePath, "Default", "Bookmarks");
        return ["https://example.com/bookmarked", "https://docs.example.org"];
    }

    public byte[] DecryptValue(byte[] encrypted, byte[] masterKey)
    {
        // Chromium v80+ uses AES-GCM with a key encrypted by DPAPI
        // Lab: just return SHA256 of input to simulate decryption
        return SHA256.HashData(encrypted.Concat(masterKey).ToArray());
    }

    private static ChromiumLogin Simulated(string url, string user)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(url + user));
        return new ChromiumLogin
        {
            Url = url, Username = user,
            PasswordHash = Convert.ToHexString(hash[..8]).ToLowerInvariant(),
            Created = DateTimeOffset.UtcNow.AddDays(-30)
        };
    }
}

public sealed class ChromiumLogin
{
    public required string Url { get; init; }
    public required string Username { get; init; }
    public string PasswordHash { get; init; } = "";
    public DateTimeOffset Created { get; init; }
}

public sealed class ChromiumCookie
{
    public required string Host { get; init; }
    public required string Name { get; init; }
    public bool Encrypted { get; init; }
    public bool HttpOnly { get; init; }
}
