using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public interface IParserExpression<T, TParserType> : IFilteredParserExpression<T, TParserType> where T : TParserType
{
    IParserExpression<T, TParserType> Where(Func<T, bool> condition);
}
