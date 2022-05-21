namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public enum InvalidStringTokenType
{
    InvalidEscape,
    NewLine,
    NoEnd
}
public record StringToken(long Start, long End, string Value) : Token(Start, End);

public record InvalidStringToken(long Start, long End, InvalidStringTokenType Type) : InvalidToken(Start, End);