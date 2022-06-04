using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Seperators;
using System;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record MethodExpression(long Start, long End, bool Export, ArgumentExpression[] Arguments, BlockExpression? Block) : Expression(Start, End)
{
    public override string ToString() => base.ToString();

    public static MethodExpression? Read(Pipe<Token> pipe, WriteLog log)
    {
        List<ArgumentExpression>? arguments = null;
        BlockExpression? block = null;
        var start = pipe.Position;
        var export = false;

        var inputParser = ParserFragment.Create()
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<KeywordToken>()
                .Where(token => token.Type == KeywordType.Export)
                .Handle(token => export = true)
                .Optional()
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
            .Call((pipe, log) =>
                {
                    List<ArgumentExpression> arguments = new();
                    ArgumentExpression? argumentBefore = null;

                    while (true)
                    {
                        ArgumentExpression? argument = null;

                        var successful = pipe.Fallback(fallbackPosition =>
                        {
                            BufferedLogger bufferedLogger = new();

                            if (argumentBefore is not null)
                            {
                                ParserFragment.Create()
                                    .Expect<WhitespaceToken>()
                                        .Optional()
                                    .Expect<SeperatorToken>()
                                        .Where(token => token.Type == SeperatorType.Value)
                                        .MessageIfMissing("',' expected.")
                                    .Expect<WhitespaceToken>()
                                        .Optional()
                                    .Parse(pipe, bufferedLogger.WriteLog);
                            }

                            argument = ArgumentExpression.Read(pipe, null);

                            if (argument.Type is null)
                            {
                                if (argument.Name is null)
                                {
                                    return false;
                                }
                                else if (argumentBefore is null)
                                {
                                    log("The first argument must declare a type.", ErrorLevel.Critical, argument.Start, argument.End);
                                }
                            }

                            if (argumentBefore is not null && argument.Type is null)
                            {
                                argument = argument with
                                {
                                    Type = argumentBefore.Type
                                };
                            }

                            argumentBefore = argument;
                            bufferedLogger.WriteTo(log);

                            return true;
                        });

                        if (successful)
                        {
                            arguments.Add(argument!);
                        }
                        else
                        {
                            return arguments;
                        }
                    }
                })
                .Handle(result => arguments = result)
                .Ok()
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<SeperatorToken>()
                .Where(token => token.Type == SeperatorType.ValueClose)
                .MessageIfMissing("')' expected.")
            .Expect<WhitespaceToken>()
                .Optional()
            .Call(BlockExpression.Read)
                .Handle(blockExpression => block = blockExpression)
                .Ok()
            .Expect<WhitespaceToken>()
                .Optional()
            .Parse(pipe, log);

        {
            List<string> names = new();

            if (arguments is not null)
            {
                foreach (var argument in arguments)
                {
                    var name = argument.Name?.Value;

                    if (names.Contains(name ?? ""))
                    {
                        log($"Duplicate argument name '{name}'", ErrorLevel.Critical, argument.Name!.Start, argument.Name.End);
                    }

                    if (name is not null)
                    {
                        names.Add(name);
                    }
                }
            }
        }

        return new(start, pipe.Position, export, arguments?.ToArray() ?? Array.Empty<ArgumentExpression>(), block);
    }
}
