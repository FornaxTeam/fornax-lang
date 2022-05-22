using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;

public interface IParserExpressionToFilter<T, TParserType> : IParserExpressionToFinish<T, TParserType> where T : TParserType
{
    IParserExpressionToFilter<T, TParserType> Where(Func<T, bool> condition);
}
