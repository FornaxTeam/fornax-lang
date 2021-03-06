using Fornax.Compiler.Logging;
using System.IO;

namespace Fornax.Compiler.Pipeline;

public class Source : Pipe<char?>
{
    private readonly string data;

    public override long Position { get; set; }

    public override bool HasNext => Position < data.Length;

    public override long Length => data.Length;

    public override char? ReadNext(WriteLog log)
    {
        if (Position >= data.Length)
        {
            return null;
        }

        var current = data[(int)Position];
        Position++;
        return current;
    }

    private Source(string data) => this.data = data;

    public static Source FromFile(string path) => new(File.ReadAllText(path));

    public static Source FromData(string data) => new(data);

    public override string ToString() => data;
}
