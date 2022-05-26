namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ImportExpression : Expression
{
    public string Library { get; private set; } = "";

    public string Path { get; private set; } = "";
}