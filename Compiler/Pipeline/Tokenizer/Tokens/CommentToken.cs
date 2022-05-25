using System.Collections.Generic;
using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;



public record CommentToken(long Start, long End, bool multiLine) : Token(Start, End)
{
    public static CommentToken? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;
        var multiLine = false;
        var @char = pipe.ReadNext();
        if (@char != '/')
            return null;
        @char = pipe.ReadNext();
        switch (@char)
        {
            case '/':
                {
                    while (pipe.HasNext)
                    {
                        @char = pipe.ReadNext();
                        if (@char == '\n')
                            break;
                    }

                    break;
                }
            case '*':
                {
                    multiLine = true;
                    while (pipe.HasNext)
                    {
                        @char = pipe.ReadNext();
                        if (@char != '*') continue;
                        @char = pipe.ReadNext();
                        if (@char == '/')
                            break;
                    }

                    break;
                }
            default:
                return null;
        }
        
        return new CommentToken(start, pipe.Position, multiLine);
    }
}
