using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public class WhitespaceToken : Token
{
    public enum WhitespaceType
    {
        Tab,
        Space,
        NewLine,
    }

    public record Whitespace(WhitespaceType Type, int Length);

    public List<Whitespace> Whitespaces { get; } = new();

    protected override bool Read(Pipe<char?> pipe)
    {
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
                        Whitespaces.Add(whitespace);
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
                        Whitespaces.Add(whitespace);
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
                        Whitespaces.Add(whitespace);
                    }

                    whitespace = new Whitespace(WhitespaceType.NewLine, 1);
                }
            }
            else
            {
                pipe.Position--;
                break;
            }
        }

        if (whitespace != null)
        {
            Whitespaces.Add(whitespace);
        }

        return Whitespaces.Count != 0;
    }
}
