using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public class Test
{
    public static void TestMethod()
    {
        var parser = Parser<Token>.Create()
            .Expect<KeywordToken>()
                .Where(token => token.Keyword == Keyword.Export)
                .Optional();
    }
}
