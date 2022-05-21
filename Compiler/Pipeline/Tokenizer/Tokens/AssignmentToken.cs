namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record AssignmentToken(long Start, long End) : Token(Start, End);
