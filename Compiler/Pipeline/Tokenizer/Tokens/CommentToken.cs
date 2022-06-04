namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public class CommentToken : Token
{
    public bool MultiLine { get; private set; } = false;

    protected override bool Read(Pipe<char?> pipe)
    {
        var @char = pipe.ReadNext();

        if (@char != '/')
        {
            return false;
        }

        @char = pipe.ReadNext();

        switch (@char)
        {
            case '/':
                while (pipe.HasNext)
                {
                    @char = pipe.ReadNext();

                    if (@char == '\n')
                    {
                        break;
                    }
                }

                break;

            case '*':
                MultiLine = true;

                while (pipe.HasNext)
                {
                    @char = pipe.ReadNext();

                    if (@char != '*')
                    {
                        continue;
                    }

                    @char = pipe.ReadNext();

                    if (@char == '/')
                    {
                        break;
                    }
                }

                break;

            default:
                return false;
        }

        return true;
    }
}
