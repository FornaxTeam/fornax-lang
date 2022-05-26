namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ArgumentExpression : Expression
{
    public string Type { get; private set; } = "";

    public string Name { get; private set; } = "";
}