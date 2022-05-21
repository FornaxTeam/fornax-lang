using System.IO;

namespace Fornax.Compiler.Pipeline;

public class Source : Pipe<char?>
{
    private readonly string data;

    public override long Position { get; set; } = 0;

    public override bool HasNext => Position < data.Length;

    public override char? ReadNext()
    {
        if (Position >= data.Length)
        {
            return null;
        }

        var current = data[(int)Position];
        Position++;
        return current;
    }

    private Source(string path) => data = File.ReadAllText(path);

    public static Source Create(string path) => new(path);
}