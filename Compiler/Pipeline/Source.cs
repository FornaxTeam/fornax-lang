using System.IO;

namespace Fornax.Compiler.Pipeline;

public class Source : Pipe<char?>
{
    private readonly string data;

    public override long Position { get; set; }

    public override bool HasNext => Position < data.Length;

    public override char? ReadNext() => HasNext ? data[(int)Position++] : null;

    private Source(string path) => data = File.ReadAllText(path);

    public static Source Create(string path) => new(path);

    public override string ToString() => data;
}
