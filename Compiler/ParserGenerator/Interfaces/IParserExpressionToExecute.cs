using Fornax.Compiler.Logging;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler.ParserGenerator.Interfaces;

public interface IParserExpressionToExecute
{
    void Execute(Pipe<Token> pipe, WriteLog log);
}