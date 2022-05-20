namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;

public record BracketToken(long Start, long End, Bracket Bracket, bool Opened) : Token(Start, End);
