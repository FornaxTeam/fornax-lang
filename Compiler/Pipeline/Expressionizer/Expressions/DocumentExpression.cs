namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record DocumentExpression(long Start, long End, ImportExpression[] Imports) : Expression(Start, End);
