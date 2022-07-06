using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock.Constants;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
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

        var readers = new Callable<IValueExpression>[]
        {
            StringConstant.Read,
            NumberConstant.Read,
            CallExpression.Read,
        };

        IValueExpression? expression = null;
        List<BufferedLogger> loggers = new();

        foreach (var reader in readers)
        {
            (var currentLogger, expression) = TryRead(reader, pipe, log);
            loggers.Add(currentLogger);

            if (expression is null)
            {
                break;
            }
        }

        if (expression == null)
        {
            loggers.MaxBy(logger => logger.CriticalCount)?.WriteTo(log);
        }

        return new(start, pipe.Position, expression);
    }

    private static (BufferedLogger bufferedLogger, T? result) TryRead<T>(Callable<T> callable, Pipe<Token> pipe, WriteLog log) where T : class, IValueExpression
    {
        BufferedLogger bufferedLogger = new();
        T? result = null;

        pipe.Fallback(l =>
        {
            result = callable(pipe, bufferedLogger.Log);
            return bufferedLogger.HasErrors;
        });
        
        return (bufferedLogger, result);
    }
}
