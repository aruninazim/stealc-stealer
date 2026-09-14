using System.Security.Cryptography;
using System.Text;

namespace Stealc.Grabber.Parsers;

public sealed class CookieDecryptor
{
    // Chromium v80+: encrypted_value prefix:
    //   v10 (Linux DPAPI), v11 (macOS Keychain), AES-GCM on Windows with DPAPI master key
    // The master key is stored in Local State JSON as os_crypt.encrypted_key (Base64, DPAPI-wrapped)

    public byte[] ExtractMasterKey(string localStatePath)
    {
        // Lab: generate deterministic fake key
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(localStatePath));
        return hash[..32];
    }

    public string DecryptCookieValue(byte[] encryptedValue, byte[] masterKey)
    {
        if (encryptedValue.Length < 15) return "(too short)";

        // Real: first 3 bytes = "v10"/"v11", next 12 = nonce, rest = ciphertext+tag
        // Lab: simulate with SHA256
        var combined = new byte[encryptedValue.Length + masterKey.Length];
        Buffer.BlockCopy(encryptedValue, 0, combined, 0, encryptedValue.Length);
        Buffer.BlockCopy(masterKey, 0, combined, encryptedValue.Length, masterKey.Length);
        var hash = SHA256.HashData(combined);
        return Convert.ToHexString(hash[..16]).ToLowerInvariant();
    }

    public bool IsDpapiWrapped(byte[] data)
    {
        // DPAPI blob starts with 01 00 00 00 D0 8C 9D DF 01 15 D1 11
        return data.Length > 12
               && data[0] == 0x01 && data[1] == 0x00
               && data[2] == 0x00 && data[3] == 0x00;
    }

    public byte[] UnwrapDpapi(byte[] wrapped)
    {
        // Lab: return SHA256 of input (real would call CryptUnprotectData)
        return SHA256.HashData(wrapped);
    }
}
