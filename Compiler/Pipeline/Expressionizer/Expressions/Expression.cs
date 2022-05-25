namespace Fornax.Compiler.Pipeline.Expressionizer;

public record Expression(long Start, long End)
{
    public long Lenght => End - Start;
}
