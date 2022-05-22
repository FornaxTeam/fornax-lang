namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ExecuteExpression : Expression
{
    public ExecuteExpression(string name, string[] args)
    {
        Name = name;
        Args = args;
    }

    public string Name { get; private set; }
    public string[] Args { get; private set; }
}