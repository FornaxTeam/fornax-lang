using Fornax.Compiler.Pipeline;
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
        static (string type, string name) ExpectParameter(Pipe<Token> pipe, WriteLog log)
        {
            var type = "";
            var name = "";

            if (!pipe.Fallback(fallbackPosition =>
            {
                return ParserFragment.Create()
                    .Expect<IdentifierToken>()
                        .Handle(token => type = token.Value)
                        .MessageIfMissing("Type expected.")
                    .Expect<WhitespaceToken>()
                        .MessageIfMissing("Whitespace expected.")
                    .Expect<IdentifierToken>()
                        .Handle(token => name = token.Value)
                        .MessageIfMissing("Parameter name expected.")
                    .Parse(pipe, null);
            }))
            {
                type = "";

                ParserFragment.Create()
                    .Expect<IdentifierToken>()
                        .Handle(token => name = token.Value)
                        .MessageIfMissing("Parameter name expected.")
                    .Parse(pipe, log);
            }

            return (type, name);
        }

        var parameter = ("", "");

        var inputParser = ParserFragment.Create()
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<IdentifierToken>()
                .MessageIfMissing("Name expected.")
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<SeperatorToken>()
                .Where(token => token.Type == SeperatorType.ValueOpen)
                .MessageIfMissing("'(' expected.")
            .Expect<WhitespaceToken>()
                .Optional()
            .Call(ExpectParameter)
                .Handle(result => parameter = result)
                .Ok()
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<SeperatorToken>()
                .Where(token => token.Type == SeperatorType.ValueClose)
                .MessageIfMissing("')' expected.")
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<SeperatorToken>()
                .Where(token => token.Type == SeperatorType.Command)
                .MessageIfMissing("';' expected.")
            .Expect<WhitespaceToken>()
                .Optional()
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

        Console.WriteLine(parameter.Item1);
        Console.WriteLine(parameter.Item2);

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
