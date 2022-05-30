using Fornax.Compiler.Pipeline;
using Fornax.Compiler.ParserGenerator.Interfaces;
using Fornax.Compiler.Pipeline.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Compiler.ParserGenerator;

public class ParserExpression<T> : IParserExpressionToCount<T>, IParserExpressionToExecute where T : Token
{
    private readonly ParserFragment parser;
    private readonly List<Func<T, bool>> conditions = new();
    private readonly List<Action<T>> handlers = new();
    private string? messageIfMissing = null;
    private (int from, int to) count = (1, 1);

    public ParserExpression(ParserFragment parser) => this.parser = parser;

    public IParserExpressionToFilter<T> Where(Func<T, bool> condition)
    {
        conditions.Add(condition);
        return this;
    }

    public IParserExpressionToFinish<T> Handle(Action<T> handler)
    {
        handlers.Add(handler);
        return this;
    }

    public IParserFragment Optional()
    {
        count = (0, count.to);
        return parser;
    }

    public IParserFragment MessageIfMissing(string message)
    {
        messageIfMissing = message;
        return parser;
    }

    public void Execute(Pipe<Token> pipe, WriteLog log)
    {
        for (var i = 0; i < count.to; i++)
        {
            var isExpected = count.from > i;

            var succsessful = pipe.Fallback(fallbackPosition =>
            {
                if (pipe.HasNext)
                {
                    if (pipe.ReadNext() is T entry)
                    {
                        if (conditions.All(condition => condition(entry)))
                        {
                            foreach (var handler in handlers)
                            {
                                handler(entry);
                            }

                            return true;
                        }
                    }
                }

                if (isExpected && messageIfMissing is not null)
                {
                    log(messageIfMissing, ErrorLevel.Critical, fallbackPosition);
                }

                return false;
            });

            if (!succsessful)
            {
                break;
            }
        }
    }

    public IParserExpressionToFilter<T> Many(int from, int to)
    {
        count = (from, to);
        return this;
    }

    public IParserExpressionToFilter<T> Many(int from) => Many(from, int.MaxValue);
}
