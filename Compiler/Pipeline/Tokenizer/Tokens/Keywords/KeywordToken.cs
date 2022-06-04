using System;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;

public class KeywordToken : Token
{
    public KeywordType Type { get; private set; }

    protected override bool Read(Pipe<char?> pipe)
    {
        foreach (var keyword in Enum.GetValues<KeywordType>())
        {
            if (pipe.Fallback(fallbackPosition =>
            {
                var keywordString = keyword.ToString().ToLower();

                for (var i = 0; i < keywordString.Length; i++)
                {
                    if (!pipe.HasNext || pipe.ReadNext()!.Value != keywordString[i])
                    {
                        return false;
                    }
                }

                return true;
            }))
            {
                Type = keyword;
                return true;
            }
        }

        return false;
    }
}
