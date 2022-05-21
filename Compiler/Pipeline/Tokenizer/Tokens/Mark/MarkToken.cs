using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Mark;

public record MarkToken(long Start, long End, Mark Mark) : Token(Start, End)
{
    public static MarkToken? Read(Pipe<char?> pipe)
    {
        if (!pipe.HasNext)
        {
            return null;
        }

        var @char = pipe.ReadNext()!.Value;

        Mark? mark = @char switch
        {
            '!' => Mark.Exclamation,
            '?' => Mark.Question,
            _ => null
        };

        return mark == null ? null : new MarkToken(pipe.Position - 1, pipe.Position, mark.Value);
    }
}