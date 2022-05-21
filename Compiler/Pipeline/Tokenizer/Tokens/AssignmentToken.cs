namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record AssignmentToken(long Start, long End) : Token(Start, End)
{
    public static AssignmentToken? Read(Pipe<char?> pipe)
    {
        var content = pipe.ReadNext();

        return content == '=' ? new AssignmentToken(pipe.Position - 1, pipe.Position) : null;
    }
}
