using System.Collections.Generic;
using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public enum WhitespaceType
{
    Tab,
    Space,
    NewLine,
}

public record Whitespace(WhitespaceType Type, int Length);

public record WhitespaceToken(long Start, long End, List<Whitespace> Whitespaces) : Token(Start, End)
{
    public static WhitespaceToken? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;
        var whitespaces = new List<Whitespace>();
        Whitespace? whitespace = null;
        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext();
            if (@char == '\t')
            {
                if (whitespace?.Type == WhitespaceType.Tab)
                {
                    whitespace = new Whitespace(whitespace.Type, whitespace.Length + 1);
                }
                else
                {
                    if (whitespace != null)
                    {
                        whitespaces.Add(whitespace);
                    }
                    whitespace = new Whitespace(WhitespaceType.Tab, 1);
                }
            }
            else if (@char == ' ')
            {
                if (whitespace?.Type == WhitespaceType.Space)
                {
                    whitespace = new Whitespace(whitespace.Type, whitespace.Length + 1);
                }
                else
                {
                    if (whitespace != null)
                    {
                        whitespaces.Add(whitespace);
                    }
                    whitespace = new Whitespace(WhitespaceType.Space, 1);
                }
            }
            else if (@char == '\n')
            {
                if (whitespace?.Type == WhitespaceType.NewLine)
                {
                    whitespace = new Whitespace(whitespace.Type, whitespace.Length + 1);
                }
                else
                {
                    if (whitespace != null)
                    {
                        whitespaces.Add(whitespace);
                    }
                    whitespace = new Whitespace(WhitespaceType.NewLine, 1);
                }
            }
            else
            {
                break;
            }
        }
        if (whitespace != null)
        {
            whitespaces.Add(whitespace);
        }
        if (whitespaces.Count == 0)
        {
            return null;
        }
        var end = pipe.Position;
        return new WhitespaceToken(start, end, whitespaces);
    }
}
