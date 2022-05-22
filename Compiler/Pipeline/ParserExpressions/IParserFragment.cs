using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public interface IParserFragment<TParserType>
{
    IParserExpression<T, TParserType> Expect<T>() where T : TParserType;

    IParserFragment<TParserType> ToParser();
}