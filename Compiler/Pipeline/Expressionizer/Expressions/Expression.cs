namespace Fornax.Compiler.Pipeline.Expressionizer;

public abstract class Expression
{
    public long Length => End - Start;
    
    public long Start { get; set; }
    public long End { get; set; }
    
    public Expression(long start, long end)
    {
        Start = start;
        End = end;
    }
}
