using Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;
using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public interface IParserFragment<TParserType>
{
    IParserExpressionToCount<T, TParserType> Expect<T>() where T : TParserType;
}