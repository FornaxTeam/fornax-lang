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

        var @char = pipe.ReadNext()!.Value;

        if (@char == '!')
        {
            @char = pipe.ReadNext()!.Value;

            if (@char == '=')
            {
                return new ComparisonToken(start, pipe.Position, Comparison.NotEqual);
            }

            return null;
        }

        if (@char == '=')
        {
            @char = pipe.ReadNext()!.Value;
            if (@char == '=')
            {
                return new ComparisonToken(start, pipe.Position, Comparison.Equal);
            }

            return null;
        }
        
        if (@char == '<')
        {
            @char = pipe.ReadNext()!.Value;
            if (@char == '=')
            {
                return new ComparisonToken(start, pipe.Position, Comparison.LessOrEqual);
            }

            return new ComparisonToken(start, pipe.Position, Comparison.Less);
        }
        
        if (@char == '>')
        {
            @char = pipe.ReadNext()!.Value;
            if (@char == '=')
            {
                return new ComparisonToken(start, pipe.Position, Comparison.GreaterOrEqual);
            }

            return new ComparisonToken(start, pipe.Position, Comparison.Greater);
        }
        
        return null;
    }

}
