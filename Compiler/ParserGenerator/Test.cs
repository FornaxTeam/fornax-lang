using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
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
        var parameter = ParserFragment.Create()
            .Block((pipe, log) =>
            {
                var isSuccessful = false;

                pipe.Fallback(() =>
                {
                    isSuccessful = ParserFragment.Create()
                        .Expect<IdentifierToken>()
                            .MessageIfMissing("Type expected.")
                        .Expect<IdentifierToken>()
                            .MessageIfMissing("Parameter name expected.")
                        .Parse(pipe, null);

                    return isSuccessful;
                });

                if (!isSuccessful)
                {
                    ParserFragment.Create()
                        .Expect<IdentifierToken>()
                            .MessageIfMissing("Parameter name expected.")
                        .Parse(pipe, log);
                }
            });

        var method = ParserFragment.Create()
            .Expect<KeywordToken>()
                .Where(token => token.Keyword == Keyword.Export)
                .Optional()
            .Expect<IdentifierToken>()
                .MessageIfMissing("Method name expected.")
            .Expect<BracketToken>()
                .Where(token => token.Bracket == Bracket.Parameter && token.Opened)
                .MessageIfMissing("'(' expected.")
            .Call(parameter)
            .Expect<BracketToken>()
                .Where(token => token.Bracket == Bracket.Parameter && !token.Opened)
                .MessageIfMissing("')' expected.")
            .Expect<BracketToken>()
                .Where(token => token.Bracket == Bracket.Block && token.Opened)
                .MessageIfMissing("'{' expected.")
            .Expect<BracketToken>()
                .Where(token => token.Bracket == Bracket.Block && !token.Opened)
                .MessageIfMissing("'}' expected.")
            .ExpectEnd();

        method.Parse(tokens, (message, errorLevel, start, end) =>
            {
                var prefix = "§6[" + errorLevel switch
                {
                    ErrorLevel.Critical => "§cCRIT",
                    ErrorLevel.Warning => "§eWARN",
                    ErrorLevel.Info => "§3INFO",
                    _ => "§7NONE"
                } + "§r]: §f";

                ColoredConsole.WriteLine(prefix + message.Replace("\n", "\n" + prefix) + $" §8[{start}, {end}]");
            });

        ColoredConsole.WriteLine("§7\nTokens:");
        foreach (var token in tokens.Finalize())
            if (token is not null)
                ColoredConsole.WriteLine("§7 - " + token);
    }
}
