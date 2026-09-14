using System.IO.Compression;
using System.Text;

namespace Stealc.Grabber.Exfil;

public sealed class ZipPacker
{
    private readonly string _outputPath;
    private readonly List<(string Name, string Content)> _entries = [];

    public ZipPacker(string outputPath) => _outputPath = outputPath;

    public void AddText(string name, string content) => _entries.Add((name, content));

    public void AddFile(string name, string sourcePath)
    {
        // Lab mode: don't actually read external files
        _entries.Add((name, $"[file reference: {sourcePath}]"));
    }

    public string Pack()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_outputPath) ?? ".");
        using var stream = File.Create(_outputPath);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

        foreach (var (name, content) in _entries)
        {
            var entry = archive.CreateEntry(name, CompressionLevel.SmallestSize);
            using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
            writer.Write(content);
        }

        return _outputPath;
    }

    public int EntryCount => _entries.Count;

    public long EstimatedSize => _entries.Sum(e => (long)Encoding.UTF8.GetByteCount(e.Content));
}
