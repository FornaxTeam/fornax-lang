using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public record NumberToken(long Start, long End, string Value, bool HasPoint) : Token(Start, End)
{
    public static NumberToken? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;
        var sb = new StringBuilder();
        var hasPoint = false;
        while (true) {
            var @char = pipe.ReadNext();
            switch (@char)
            {
                case ' ':
                    continue;
                case '.' when hasPoint:
                    return null;
                case '.':
                    hasPoint = true;
                    sb.Append(@char);
                    continue;
            }

            if (@char != null && char.IsDigit(@char.Value)) {
                sb.Append(@char);
                continue;
            }
            
            if (@char != null)
            {
                pipe.Position--;
            }
            return sb.Length == 0 || sb.Equals(".") ? null : new NumberToken(start, pipe.Position, sb.ToString(), hasPoint);
        }
    }
}
