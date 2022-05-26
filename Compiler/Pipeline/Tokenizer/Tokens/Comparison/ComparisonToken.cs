using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Comparison;

public record ComparisonToken(long Start, long End, Comparison Comparison) : Token(Start, End)
{
   public static ComparisonToken? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;

        if (!pipe.HasNext)
        {
            return null;
        }

        var @char = pipe.ReadNext();

        switch (@char)
        {
            case '!':
                @char = pipe.ReadNext();
                return @char is '=' ? new ComparisonToken(start, pipe.Position, Comparison.NotEqual) : null;
            case '=':
                @char = pipe.ReadNext();
                return @char is '=' ? new ComparisonToken(start, pipe.Position, Comparison.Equal) : null;
            case '<':
                @char = pipe.ReadNext();
                return @char is '=' ? new ComparisonToken(start, pipe.Position, Comparison.LessOrEqual) : new ComparisonToken(start, pipe.Position, Comparison.Less);
        }

        if (@char is not '>') return null;
        @char = pipe.ReadNext();
        return @char is '=' ? new ComparisonToken(start, pipe.Position, Comparison.GreaterOrEqual) : new ComparisonToken(start, pipe.Position, Comparison.Greater);
    }

}
