using Fornax.Compiler.ParserGenerator.Interfaces;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.ParserExpressions;
using Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;
using Fornax.Compiler.Pipeline.Tokenizer;
using System;
using System.Collections.Generic;

namespace Fornax.Compiler.ParserExpressions;

public interface IParserFragment
{
    IReadOnlyList<IParserExpressionToExecute> Expressions { get; }

    IParserExpressionToCount<T> Expect<T>() where T : Token;

    IParserFragment Call(IParserFragment fragment);

    IParserFragment Block(Action<Pipe<Token>, WriteLog> handle);

    IParserFragment ExpectEnd();

    bool Parse(Pipe<Token> pipe, WriteLog? log);
}