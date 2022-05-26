using Fornax.Compiler.Pipeline.Tokenizer;
using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;

public interface IParserExpressionToFilter<T> : IParserExpressionToFinish<T> where T : Token
{
    IParserExpressionToFilter<T> Where(Func<T, bool> condition);
}
