using System.Security.Cryptography;
using System.Text;

namespace Stealc.Grabber.Exfil;

public sealed class Fingerprint
{
    public string MachineId { get; }
    public string ShortId { get; }
    public DateTimeOffset FirstSeen { get; }

    public Fingerprint()
    {
        var material = string.Join("|",
            Environment.MachineName,
            Environment.UserName,
            Environment.ProcessorCount,
            Environment.OSVersion.Platform);

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        MachineId = Convert.ToHexString(hash).ToLowerInvariant();
        ShortId = MachineId[..16];
        FirstSeen = DateTimeOffset.UtcNow;
    }

    public bool IsDuplicate(string previousId) =>
        string.Equals(ShortId, previousId, StringComparison.OrdinalIgnoreCase);

    public override string ToString() => ShortId;
}
