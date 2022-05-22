using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;

public interface IParserExpressionToFinish<T, TParserType> where T : TParserType
{
    IParserExpressionToFinish<T, TParserType> Handle(Action<T> handler);

    IParserFragment<TParserType> Optional();

    IParserFragment<TParserType> MessageIfMissing(string message);

    ParserExpressionResult Execute(Pipe<TParserType> pipe);
}
