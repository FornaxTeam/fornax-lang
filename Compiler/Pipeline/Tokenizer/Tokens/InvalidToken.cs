using Fornax.Compiler.Logging;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public class InvalidToken : Token
{
    protected override bool Read(Pipe<char?> pipe, WriteLog log) => false;

    public InvalidToken(long start, long end)
    {
        Start = start;
        End = end;
    }
}

