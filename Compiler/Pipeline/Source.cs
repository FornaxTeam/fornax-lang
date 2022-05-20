using System.IO;

namespace Fornax.Compiler.Pipeline;

public class Source : Pipe<char>
{
    private readonly string data;

    public override long Position { get; set; } = 0;

    public override char? ReadNext() => Position >= data.Length ? null : data[(int)Position++];

    private Source(string path) => data = File.ReadAllText(path);

    public static Source Create(string path) => new(path);
}
