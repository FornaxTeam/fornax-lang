namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ImportExpression : Expression
{
    public ImportExpression(long start, long end, string library, string path) : base(start, end)
    {
        Library = library;
        Path = path;
    }

    public string Library { get; private set; } = "";

    public string Path { get; private set; } = "";
}