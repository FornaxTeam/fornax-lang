using Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public class ParserExpression<T, TParserType> : IParserExpressionToCount<T, TParserType> where T : TParserType
{
    private readonly ParserFragment<TParserType> parser;
    private readonly List<Func<T, bool>> conditions = new();
    private readonly List<Action<T>> handlers = new();
    private string? messageIfMissing = null;
    private bool isOptional = false;
    private (int from, int to) count = (1, 1);

    public ParserExpression(ParserFragment<TParserType> parser) => this.parser = parser;

    public IParserExpressionToFilter<T, TParserType> Where(Func<T, bool> condition)
    {
        conditions.Add(condition);
        return this;
    }

    public IParserExpressionToFinish<T, TParserType> Handle(Action<T> handler)
    {
        handlers.Add(handler);
        return this;
    }

    public IParserFragment<TParserType> Optional()
    {
        isOptional = true;
        return parser;
    }

    public IParserFragment<TParserType> MessageIfMissing(string message)
    {
        messageIfMissing = message;
        return parser;
    }

    public ParserExpressionResult Execute(Pipe<TParserType> pipe)
    {
        ParserExpressionResult? result = null;

        pipe.Fallback(() =>
        {
            if (pipe.HasNext
                && pipe.ReadNext() is T entry
                && conditions.All(condition => condition(entry)))
            {
                foreach (var handler in handlers)
                {
                    handler(entry);
                }

                result = new();
                return true;
            }

            result = new(isOptional ? null : messageIfMissing);
            return true;
        });

        return result!;
    }

    public IParserExpressionToFilter<T, TParserType> Many(int from, int to)
    {
        count = (from, to);
        return this;
    }

    public IParserExpressionToFilter<T, TParserType> Many(int from) => Many(from, int.MaxValue);
}
