using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public interface IFilteredParserExpression<T, TParserType> where T : TParserType
{
    IFilteredParserExpression<T, TParserType> Handle(Action<T> handler);

    IParserFragment<TParserType> Optional();

    IParserFragment<TParserType> MessageIfMissing(string message);

    ParserExpressionResult Execute(Pipe<TParserType> pipe);
}
