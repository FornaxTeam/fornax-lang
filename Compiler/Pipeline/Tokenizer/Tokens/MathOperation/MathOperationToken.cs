using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.MathOperation;

public record MathOperationToken(long Start, long End, MathOperation MathOperation) : Token(Start, End)
{
    public static MathOperationToken? Read(Pipe<char?> pipe)
    {
        if (!pipe.HasNext)
        {
            return null;
        }

        var @char = pipe.ReadNext()!.Value;

        MathOperation? mathOperation = @char switch
        {
            '+' => MathOperation.Plus,
            '-' => MathOperation.Minus,
            '*' => MathOperation.Multiply,
            '/' => MathOperation.Divide,
            '^' => MathOperation.Power,
            '%' => MathOperation.Modulo,
            '\\' => MathOperation.Root,
            _ => null
        };

        return mathOperation == null ? null : new MathOperationToken(pipe.Position - 1, pipe.Position, mathOperation.Value);
    }
}