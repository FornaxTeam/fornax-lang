using Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public class ParserFragment<TParserType> : IParserFragment<TParserType>
{
    public List<object> Expressions { get; } = new();

    public IParserExpressionToCount<T, TParserType> Expect<T>() where T : TParserType
    {
        var expression = new ParserExpression<T, TParserType>(this);
        Expressions.Add(expression);
        return expression;
    }

    public static IParserFragment<TParserType> Create() => new ParserFragment<TParserType>();
}
