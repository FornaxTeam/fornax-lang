using Fornax.Compiler.Pipeline.Tokenizer;
using System;

namespace Fornax.Compiler.ParserGenerator.Interfaces;

public interface IParserExpressionToFinish<T> where T : Token
{
    IParserExpressionToFinish<T> Handle(Action<T> handler);

    IParserFragment Optional();

    IParserFragment MessageIfMissing(string message);
}
