using Fornax.Compiler.Logging;
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

        var token = pipe.ReadNext((_, _, _, _) => { });

        pipe.Position = oldPosition;

        return token;
    }

    public bool Parse(Pipe<Token> pipe, WriteLog log)
    {
        BufferedLogger bufferedLogger = new();

        foreach (var expression in Expressions)
        {
            expression.Execute(pipe, (message, errorLevel, start, end)
                => bufferedLogger.Log(message, errorLevel, start, end != -1 ? end : GetTokenAt(pipe, start)?.End ?? pipe.Length));
        }

        return bufferedLogger.WriteTo(log);
    }

    public static IParserFragment Create() => new ParserFragment();

    public IParserExpressionToCount<T> Expect<T>() where T : Token
    {
        var expression = new ParserExpression<T>(this);
        Expressions.Add(expression);
        return expression;
    }

    public IParserFragment Call(IParserFragment fragment)
    {
        Expressions.Add(new Caller((pipe, log) => fragment.Parse(pipe, log)));
        return this;
    }

    private class CallResult<T> : ICallResult<T>
    {
        private readonly IParserFragment fragment;
        private readonly List<Action<T>> handlers = new();

        public CallResult(IParserFragment fragment) => this.fragment = fragment;

        public ICallResult<T> Handle(Action<T> handler)
        {
            handlers.Add(handler);
            return this;
        }

        public void Execute(T obj)
        {
            foreach (var handler in handlers)
            {
                handler(obj);
            }
        }

        public IParserFragment Ok() => fragment;
    }

    private record Caller(Action<Pipe<Token>, WriteLog> ExecuteFunc) : IParserExpressionToExecute
    {
        public void Execute(Pipe<Token> pipe, WriteLog log) => ExecuteFunc(pipe, log);
    }

    public ICallResult<T> Call<T>(Callable<T> callable)
    {
        var result = new CallResult<T>(this);
        Expressions.Add(new Caller((pipe, log) => result.Execute(callable(pipe, log))));
        return result;
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
