namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record ArrowToken(long Start, long End) : Token(Start, End)
{
    public static ArrowToken? Read(Pipe<char?> pipe)
    {
        var content = pipe.ReadNext();

        if (content != '=') return null;
        content = pipe.ReadNext();
        return content == '>' ? new ArrowToken(pipe.Position - 2, pipe.Position) : null;
    }
}