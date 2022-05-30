using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Literals;

public class StringToken : Token
{
    public string Value { get; set; } = "";

    protected override bool Read(Pipe<char?> pipe)
    {
        if (!pipe.HasNext || pipe.ReadNext()!.Value != '\"')
        {
            return false;
        }

        StringBuilder stringBuilder = new();
        var escape = false;

        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext()!.Value;

            if (escape)
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
                        return false;
                }

                escape = false;
                continue;
            }
            else if (@char == '\\')
            {
                escape = true;
                continue;
            }
            else if (@char == '\"')
            {
                Value = stringBuilder.ToString();
                return true;
            }

            if (!char.IsControl(@char))
            {
                stringBuilder.Append(@char);
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
