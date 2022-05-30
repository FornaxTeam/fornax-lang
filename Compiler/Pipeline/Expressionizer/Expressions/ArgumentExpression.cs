namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record ArgumentExpression(long Start, long End, string Type, string Name) : Expression(Start, End);
