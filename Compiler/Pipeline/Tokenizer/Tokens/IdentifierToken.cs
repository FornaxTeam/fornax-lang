namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record IdentifierToken(long Start, long End, string Name) : Token(Start, End);
