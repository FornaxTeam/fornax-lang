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

    Console.WriteLine(token);
}

Console.ReadKey();
