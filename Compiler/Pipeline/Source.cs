using System.IO;

namespace Fornax.Compiler.Pipeline;

public class Source : Pipe<char?>
{
    private readonly StreamReader _streamReader;

    public override long Position
    {
        get => _streamReader.BaseStream.Position;
        set
        {
            _streamReader.BaseStream.Position = value;
            _streamReader.DiscardBufferedData();
        }
    }

    public override bool HasNext => _streamReader.Peek() != -1;

    public override char? ReadNext()
    {
        var c = _streamReader.Read();
        if(c == -1)
            return null;
        Position++;
        return (char)c;
    }

    private Source(string path)
    {
        _streamReader = new StreamReader(path);
    }

    public static Source Create(string path) => new(path);

    public override string? ToString() => _streamReader.ToString();
}
