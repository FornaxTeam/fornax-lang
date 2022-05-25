namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ExecuteExpression : Expression
{
    public ExecuteExpression(long start = default, long end = default, string name = null, string[] args = null) : base(start, end)
    {
        Name = name;
        Args = args;
    }

    public string Name { get; private set; }
    public string[] Args { get; private set; }
}