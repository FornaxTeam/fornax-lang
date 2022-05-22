namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ImportExpression : Expression
{
    public ImportExpression(string library, string path)
    {
        Library = library;
        Path = path;
    }

    public string Library { get; private set; }
    public string Path { get; private set; }
}