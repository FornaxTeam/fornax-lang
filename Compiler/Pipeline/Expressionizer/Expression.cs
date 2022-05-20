namespace Fornax.Compiler.Pipeline.Expressionizer;

public abstract record Expression(long Start, long End)
{
    public long Lenght => End - Start;
}
