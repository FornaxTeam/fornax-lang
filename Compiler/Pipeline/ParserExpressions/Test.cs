using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public static class Test
{
    public static void Main()
    {
        var parser = Parser<Token>.Create((start, end, errorLevel, message) => Console.WriteLine(message))
            .Expect<KeywordToken>()
                .Where(token => token.Keyword == Keyword.Export)
                .Optional();

        Console.ReadKey();
    }
}
