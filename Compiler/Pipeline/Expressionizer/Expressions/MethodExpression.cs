using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class MethodExpression : Expression
{
    public long Start { get; private set; }
    public long End { get; private set; }
    public bool Export { get; private set; }
    
    public MethodExpression(long start, long end, bool export) {
        Start = start;
        End = end;
        Export = export;
    }
}
