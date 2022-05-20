using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record EndOfCommandToken(long Start, long End) : Token(Start, End);
