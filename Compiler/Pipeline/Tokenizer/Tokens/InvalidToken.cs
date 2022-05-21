namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record InvalidToken(long Start, long End) : Token(Start, End);