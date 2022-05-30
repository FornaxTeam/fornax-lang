using Fornax.Compiler.ParserGenerator.Interfaces;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;
using System;
using System.Collections.Generic;

namespace Fornax.Compiler.ParserGenerator;

public interface IParserFragment
{
    IReadOnlyList<IParserExpressionToExecute> Expressions { get; }

    IParserExpressionToCount<T> Expect<T>() where T : Token;

    ICallResult<T> Call<T>(Callable<T> fragment);

    IParserFragment ExpectEnd();

    bool Parse(Pipe<Token> pipe, WriteLog? log);
}