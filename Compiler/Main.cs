using System;

using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;

var tokens = Source.Create("Script.fdx")
    .Step(new TokenizerStep())
    .Finalize();

foreach (var token in tokens)
{
    if (token is null)
    {
        break;
    }

    Console.WriteLine(token.GetType().Name);

    if (token is IdentifierToken identifierToken)
    {
        Console.WriteLine($"> \"{identifierToken.Name}\"");
    }

    if (token is KeywordToken keywordToken)
    {
        Console.WriteLine("> " + keywordToken.Keyword);
    }

    if (token is BracketToken bracketToken)
    {
        Console.WriteLine("> " + bracketToken.Bracket + " (" + (bracketToken.Opened ? "opened" : "closed") + ")");
    }
}

Console.ReadKey();
