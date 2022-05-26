namespace Fornax.Compiler.Pipeline.Expressionizer;

public abstract class Expression
{
    public long Start { get; set; }

    public long End { get; set; }

    public long Lenght => End - Start;
}