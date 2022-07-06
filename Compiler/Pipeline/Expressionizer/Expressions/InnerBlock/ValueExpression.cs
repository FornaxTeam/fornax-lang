using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock.Constants;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock;

public record ValueExpression(long Start, long End, IValueExpression? Expression) : Expression(Start, End), IValueExpression, ICommandExpression
{
    public override string ToString() => base.ToString();

    public static ValueExpression Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        IValueExpression? expression = TryRead(StringConstant.Read, pipe, log);
        expression ??= TryRead(NumberConstant.Read, pipe, log);
        return new(start, pipe.Position, expression);
    }

    private static T? TryRead<T>(Callable<T> callable, Pipe<Token> pipe, WriteLog log) where T : class, IValueExpression
    {
        try
        {
            return callable.Invoke(pipe, log);
        }
        catch
        {
            return null;
        }
    }
}
