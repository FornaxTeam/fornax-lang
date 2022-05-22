namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ArgumentExpression : Expression
{
    public ArgumentExpression(string type, string name)
    {
        Type = type;
        Name = name;
    }

    public string Type { get; private set; }
    public string Name { get; private set; }
}