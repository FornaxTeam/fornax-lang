using System;
using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;

public record KeywordToken(long Start, long End, Keyword Keyword) : Token(Start, End)
{
    public static Keyword? GetKeyword(string text)
    {
        foreach (var keyword in Enum.GetValues<Keyword>())
        {
            if (keyword.ToString().ToLower() == text)
            {
                return keyword;
            }
        }

        return null;
    }
    
    
    
    public static KeywordToken? Read(Pipe<char?> pipe)
    {
        var identifier = IdentifierToken.Read(pipe);

        if (identifier == null || identifier.Length == 0)
        {
            return null;
        }

        var keyword = KeywordToken.GetKeyword(identifier.Name);

        return keyword.HasValue
            ? new KeywordToken(identifier.Start, identifier.End, keyword.Value)
            : null;
    }

}
