using Fornax.Compiler.Logging;
using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public class IdentifierToken : Token
{
    public string Value { get; set; } = "";

    protected override bool Read(Pipe<char?> pipe, WriteLog log)
    {
        StringBuilder stringBuilder = new();

        if (pipe.HasNext)
        {
            var @char = pipe.ReadNext(log)!.Value;

            if (char.IsLetter(@char) || @char == '_')
            {
                stringBuilder.Append(@char);
            }
            else
            {
                return false;
            }
        }

        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext(log)!.Value;

            if (char.IsLetter(@char) || char.IsDigit(@char) || @char == '_')
            {
                stringBuilder.Append(@char);
            }
            else
            {
                pipe.Position--;
                Value = stringBuilder.ToString();
                return true;
            }
        }

        Value = stringBuilder.ToString();
        return true;
    }
}
