using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class DocumentExpression : Expression
{
    public DocumentExpression(long start, long end, List<ImportExpression> imports) : base(start, end) => Imports = imports;

    public List<ImportExpression> Imports { get; private set; }
}