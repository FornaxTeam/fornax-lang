using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class MethodExpression : Expression
{
    public List<ArgumentExpression> Arguments { get; } = new();
}
