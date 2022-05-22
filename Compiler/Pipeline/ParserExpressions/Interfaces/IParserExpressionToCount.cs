namespace Fornax.Compiler.Pipeline.ParserExpressions.Interfaces;

public interface IParserExpressionToCount<T, TParserType> : IParserExpressionToFilter<T, TParserType> where T : TParserType
{
    IParserExpressionToFilter<T, TParserType> Many(int from, int to);

    IParserExpressionToFilter<T, TParserType> Many(int from);
}
