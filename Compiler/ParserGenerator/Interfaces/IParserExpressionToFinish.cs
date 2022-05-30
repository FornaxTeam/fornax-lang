using System;

namespace Fornax.Compiler.ParserGenerator.Interfaces;

public interface IParserExpressionToFinish<T>
{
    IParserFragment Optional();

    IParserFragment MessageIfMissing(string message);

    IParserExpressionToFinish<T> Handle(Action<T> handler);
}
