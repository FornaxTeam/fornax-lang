using System;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Expressionizer;
using Fornax.Compiler.Pipeline.Tokenizer;

var source = Source.Create("Script.fdx");

Console.WriteLine();
Console.WriteLine("Source:");
Console.WriteLine();

Console.WriteLine("    " + source.ToString().Replace("\r\n", "\n").Replace("\n", "\n    "));

Console.ReadKey();

var tokens = source
    .Step(new TokenizerStep());

Console.WriteLine();
Console.WriteLine("Tokens:");
Console.WriteLine();

foreach (var token in tokens.Finalize())
{
    if (token is null)
    {
        break;
    }

    Console.WriteLine(" - " + token);
}

Console.ReadKey();

Console.WriteLine();
Console.WriteLine("Expressions:");
Console.WriteLine();

var expressions = tokens
    .Step(new ExpressionizerStep());

foreach (var expression in expressions.Finalize())
{
    if (expression is null)
    {
        break;
    }

    Console.WriteLine(expression);
}

Console.ReadKey();
