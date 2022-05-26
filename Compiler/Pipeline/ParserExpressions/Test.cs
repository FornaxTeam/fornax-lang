using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public static class Test
{
	public static void _Main()
	{
		var block = ParserFragment<Token>.Create()
			.Expect<BracketToken>()
				.Where(token => token.Bracket == Bracket.Block && token.Opened)
				.MessageIfMissing("Opening block bracket expected.")
			.Expect<BracketToken>()
				.Where(token => token.Bracket == Bracket.Block && !token.Opened)
				.MessageIfMissing("Closing block bracket expected.");

		var export = false;
		var name = "";

        var method = (ParserFragment<Token>)ParserFragment<Token>.Create()
            .Expect<KeywordToken>()
                .Where(token => token.Keyword == Keyword.Export)
                .Handle(_ => export = true)
                .Optional()
            .Expect<IdentifierToken>()
                .Handle(token => name = token.Name)
                .MessageIfMissing("Method name expected.")
            .Expect<BracketToken>()
                .Where(token => token.Bracket == Bracket.Parameter && token.Opened)
                .MessageIfMissing("Opening parameter bracket expected.")
            .Expect<BracketToken>()
                .Where(token => token.Bracket == Bracket.Parameter && !token.Opened)
                .MessageIfMissing("Closing parameter bracket expected.");

		Console.WriteLine(method.Expressions.Count);

		Console.ReadKey();
	}
}
