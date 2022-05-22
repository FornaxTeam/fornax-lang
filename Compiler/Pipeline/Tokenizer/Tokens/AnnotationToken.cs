namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record AnnotationToken(long Start, long End) : Token(Start, End)
{
    public static AnnotationToken? Read(Pipe<char?> pipe)
        => pipe.ReadNext() == '@' ? new AnnotationToken(pipe.Position - 1, pipe.Position) : null;
}
