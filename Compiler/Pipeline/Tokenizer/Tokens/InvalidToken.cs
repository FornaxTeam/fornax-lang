namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public enum InvalidTokenType
{
    Backslash
}
public record InvalidToken(long Start, long End, InvalidTokenType Type) : Token(Start, End);