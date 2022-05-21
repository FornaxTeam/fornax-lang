using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public enum InvalidStringTokenType
{
    InvalidEscape,
    NewLine,
    NoEnd
}

public record StringToken(long Start, long End, string Value) : Token(Start, End)
{
    public static Token? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;
        var @char = pipe.ReadNext();

        if (@char != '"')
        {
            return null;
        }

        StringBuilder stringBuilder = new();
        var escaped = false;

        while (pipe.HasNext)
        {
            @char = pipe.ReadNext()!.Value;
            if (escaped)
            {
                switch (@char)
                {
                    case 'n':
                        stringBuilder.Append('\n');
                        break;
                    case 't':
                        stringBuilder.Append('\t');
                        break;
                    case 'r':
                        stringBuilder.Append('\r');
                        break;
                    case '"':
                        stringBuilder.Append('"');
                        break;
                    case '\\':
                        stringBuilder.Append('\\');
                        break;
                    default:
                        return new InvalidStringToken(start, pipe.Position, InvalidStringTokenType.InvalidEscape);
                }

                escaped = false;

                continue;
            }
            switch (@char)
            {
                case '\\':
                    escaped = true;
                    break;
                case '"':
                    return new StringToken(start, pipe.Position, stringBuilder.ToString());
                case '\n':
                    return new InvalidStringToken(start, pipe.Position, InvalidStringTokenType.NewLine);
                default:
                    stringBuilder.Append(@char);
                    break;
            }
        }
 
        return new InvalidStringToken(start, pipe.Position, InvalidStringTokenType.NoEnd);
    }
}

public record InvalidStringToken(long Start, long End, InvalidStringTokenType Type) : InvalidToken(Start, End);