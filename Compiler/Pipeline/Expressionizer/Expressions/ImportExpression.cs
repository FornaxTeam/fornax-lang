namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record ImportExpression(long Start, long End, string Package, string Module) : Expression(Start, End);
