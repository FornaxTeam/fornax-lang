using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;

public record BracketToken(long Start, long End, Bracket Bracket, bool Opened) : Token(Start, End)
{
   public static BracketToken? Read(Pipe<char?> pipe)
    {
        var start = pipe.Position;

        if (!pipe.HasNext)
        {
            return null;
        }

        var @char = pipe.ReadNext()!.Value;

        var opened = new[] { '(', '{', '[' }.Contains(@char);
        Bracket bracket;

        switch (@char)
        {
            case '(':
            case ')':
                bracket = Bracket.Parameter;
                break;
            case '[':
            case ']':
                bracket = Bracket.Array;
                break;
            case '{':
            case '}':
                bracket = Bracket.Block;
                break;
            default:
                return null;
        }

        return new BracketToken(start, start + 1, bracket, opened);
    }

}
