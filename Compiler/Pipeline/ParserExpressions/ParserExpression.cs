using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public record ParserExpression<T, TParserType>(Parser<TParserType> Parser, Func<T, bool>[] Conditions, Action<T>[] Handlers, bool IsOptional)
    where T : TParserType
{
    public ParserExpression<T, TParserType> Handle(Action<T> handler) => this with
    {
        Handlers = Handlers
            .Append(handler)
            .ToArray()
    };

    public ParserExpression<T, TParserType> Optional() => this with
    {
        IsOptional = true
    };

    public ParserExpression<T, TParserType> Where(Func<T, bool> condition) => this with
    {
        Conditions = Conditions
            .Append(condition)
            .ToArray()
    };
}
