using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class DocumentExpression : Expression
{
    public DocumentExpression(List<ImportExpression> imports)
    {
        Imports = imports;
    }

    public List<ImportExpression> Imports { get; private set; }
}