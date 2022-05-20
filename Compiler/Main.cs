using System;

using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;

var tokens = Source.Create("Script.fdx")
    .Step(new TokenizerStep())
    .Finalize();

foreach (var token in tokens)
{
    if (!token.HasValue)
    {
        break;
    }

    Console.WriteLine(token.Value.Type);
}
