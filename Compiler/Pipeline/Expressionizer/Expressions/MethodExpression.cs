namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record MethodExpression(long Start, long End, ArgumentExpression[] Arguments) : Expression(Start, End);
