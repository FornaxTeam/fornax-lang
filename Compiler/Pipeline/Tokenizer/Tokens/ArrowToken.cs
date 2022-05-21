namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record ArrowToken(long Start, long End) : Token(Start, End);
