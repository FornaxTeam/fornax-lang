using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record EndOfCommandToken(long Start, long End) : Token(Start, End)
{
    public static EndOfCommandToken? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;
        var @char = pipe.ReadNext();

        if (@char == ';')
        {
            return new EndOfCommandToken(start, start + 1);
        }

        return null;
    }

}
