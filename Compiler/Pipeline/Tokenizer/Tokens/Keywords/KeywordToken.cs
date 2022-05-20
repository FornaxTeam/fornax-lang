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
}
