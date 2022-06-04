using Fornax.Compiler.Logging;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Seperators;
using System;

namespace Fornax.Compiler.ParserGenerator;

public static class Test
{
    public static void Main()
    {
        while (true)
        {
            ColoredConsole.Write("§e> §7");

            var tokens = Source.FromData(Console.ReadLine()!)
                .Step(new TokenizerStep());

            try
            {
                PrintLog(tokens);
            }
            catch (Exception e)
            {
                ColoredConsole.WriteLine("§c" + e);
            }
        }
    }

    public static void PrintLog(Pipe<Token> tokens)
    {
        MethodExpression? method = null;

        var inputParser = ParserFragment.Create()
            .Call(MethodExpression.Read)
                .Handle(result => method = result)
                .Ok()
            .ExpectEnd();

        var errorLineTop = Console.CursorTop;

        Console.WriteLine();

        inputParser.Parse(tokens, (message, errorLevel, start, end) =>
        {
            var prefix = "§f[" + errorLevel switch
            {
                ErrorLevel.Critical => "§cCRIT",
                ErrorLevel.Warning => "§eWARN",
                ErrorLevel.Info => "§3INFO",
                _ => "§7NONE"
            } + "§r]: §7";

            if (start == end)
            {
                end++;
            }

            ColoredConsole.Write("§c" + new string('~', (int)(end - start)), (int)(2 + start), errorLineTop);

            ColoredConsole.WriteLine(prefix + message.Replace("\n", "\n" + prefix) + $" §8({start}, {end})");
        });

        Console.WriteLine(method);

        ColoredConsole.WriteLine("§7Tokens:");

        foreach (var token in tokens.Finalize())
        {
            if (token is not null)
            {
                ColoredConsole.WriteLine("§7 - " + token);
            }
        }
    }
}
