using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class DocumentExpression : Expression
{
    public List<ImportExpression> Imports { get; } = new();
}