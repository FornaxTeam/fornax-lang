namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ArgumentExpression : Expression
{
    public ArgumentExpression(long start, long end, string type, string name) : base(start, end)
    {
        Type = type;
        Name = name;
    }

    public string Type { get; private set; } = "";

    public string Name { get; private set; } = "";
}