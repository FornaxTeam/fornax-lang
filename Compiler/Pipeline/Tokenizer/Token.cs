namespace Fornax.Compiler.Pipeline.Tokenizer;

public record Token(long Start, long End)
{
    public long Length => End - Start;
}
