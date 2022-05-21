using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record IdentifierToken(long Start, long End, string Name) : Token(Start, End)
{
    public static IdentifierToken? Read(Pipe<char?> pipe)
    {
        StringBuilder stringBuilder = new();
        var start = pipe.Position;

        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext()!.Value;

            if (!char.IsLetter(@char))
            {
                pipe.Position--;
                return stringBuilder.Length == 0 ? null : new IdentifierToken(start, pipe.Position, stringBuilder.ToString());
            }

            stringBuilder.Append(@char);
        }
        
        return stringBuilder.Length > 0 ? new IdentifierToken(start, pipe.Position, stringBuilder.ToString()) : null;
    }

}
