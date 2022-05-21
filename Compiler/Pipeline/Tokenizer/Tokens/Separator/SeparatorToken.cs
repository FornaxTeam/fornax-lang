using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separator;

public record SeparatorToken(long Start, long End, Separator Separator) : Token(Start, End)
{
    public static SeparatorToken? Read(Pipe<char?> pipe)
    {
        if (!pipe.HasNext)
        {
            return null;
        }

        var @char = pipe.ReadNext()!.Value;

        Separator? separator = @char switch
        {
            '.' => Separator.Point,
            ':' => Separator.Collection,
            _ => null
        };

        return separator == null ? null : new SeparatorToken(pipe.Position - 1, pipe.Position, separator.Value);
    }
}