using Fornax.Compiler.ParserGenerator.Interfaces;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Compiler.ParserGenerator;

public class ParserFragment : IParserFragment
{
    public List<IParserExpressionToExecute> Expressions { get; } = new();

    IReadOnlyList<IParserExpressionToExecute> IParserFragment.Expressions => Expressions;

    private static Token? GetTokenAt(Pipe<Token> pipe, long position)
    {
        var oldPosition = pipe.Position;
        pipe.Position = position;

        var token = pipe.ReadNext();

        pipe.Position = oldPosition;

        return token;
    }

    public bool Parse(Pipe<Token> pipe, WriteLog? log)
    {
        List<(string message, ErrorLevel errorLevel, long start, long end)> logEntries = new();

        foreach (var expression in Expressions)
        {
            expression.Execute(pipe, (message, errorLevel, start, end)
                => logEntries.Add((message, errorLevel, start, end != -1 ? end : GetTokenAt(pipe, start)?.End ?? pipe.Length)));
        }

        var unterline = new ErrorLevel[pipe.Length + 1];

        foreach (var (_, errorLevel, start, end) in logEntries)
        {
            for (var i = start; i < end; i++)
            {
                unterline[i] = errorLevel;
            }

            if (start == end)
            {
                unterline[start] = errorLevel;
            }
        }

        if (log is not null)
        {
            foreach (var (message, errorLevel, start, end) in logEntries)
            {
                log(message, errorLevel, start, end);
            }
        }

        return !logEntries.Any(entry => entry.errorLevel == ErrorLevel.Critical);
    }

    public static IParserFragment Create() => new ParserFragment();

    public IParserExpressionToCount<T> Expect<T>() where T : Token
    {
        var expression = new ParserExpression<T>(this);
        Expressions.Add(expression);
        return expression;
    }

    private record Caller(Action<Pipe<Token>, WriteLog> ExecuteFunc) : IParserExpressionToExecute
    {
        public void Execute(Pipe<Token> pipe, WriteLog log) => ExecuteFunc(pipe, log);
    }

    public IParserFragment Call(IParserFragment fragment)
    {
        Expressions.Add(new Caller((pipe, log) => fragment.Parse(pipe, log)));
        return this;
    }

    public IParserFragment Block(Action<Pipe<Token>, WriteLog> body)
    {
        Expressions.Add(new Caller((pipe, log) => body(pipe, log)));
        return this;
    }

    public IParserFragment ExpectEnd()
    {
        Expressions.Add(new Caller((pipe, log) =>
        {
            if (pipe.HasNext)
            {
                log("End expected.", ErrorLevel.Critical, pipe.Position, pipe.Length);
            }
        }));

        return this;
    }
}
