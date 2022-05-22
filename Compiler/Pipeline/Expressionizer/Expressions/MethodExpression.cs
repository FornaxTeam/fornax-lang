using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class MethodExpression : Expression
{
    public MethodExpression(List<ArgumentExpression> arguments) => Arguments = arguments;

    public List<ArgumentExpression> Arguments { get; private set; }
}
