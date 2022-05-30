using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler.ParserGenerator.Interfaces;

public interface IParserExpressionToCount<T> : IParserExpressionToFilter<T> where T : Token
{
    IParserExpressionToFilter<T> Many(int from, int to);

    IParserExpressionToFilter<T> Many(int from);
}
