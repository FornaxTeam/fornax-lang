using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public class KeywordToken : Token
{
    private readonly string[] keywords =
    {
        "import",
        "export",

        "struct",
        "interface",
        "implement",
        "enum",

        "const",
        "final",
        "var",

        "return",
        "break",
        "continue",

        "if",
        "while",
        "for",
        "match",

        "as",
        "is",
    };

    protected override bool Read(Pipe<char?> pipe)
    {
        foreach (var keyword in keywords)
        {
            if (pipe.Fallback(fallbackPosition =>
            {
                for (var i = 0; i < keyword.Length; i++)
                {
                    if (!pipe.HasNext || pipe.ReadNext()!.Value != keyword[i])
                    {
                        return false;
                    }
                }

                return true;
            }))
            {
                return true;
            }
        }

        return false;
    }
}
