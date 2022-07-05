using Fornax.Compiler.Logging;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Literals;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Seperators;
using System;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public class TokenizerStep : IPipeStep<char?, Token>
{
    private static readonly Func<Pipe<char?>, WriteLog, Token?>[] readers = new Func<Pipe<char?>, WriteLog, Token?>[]
    {
        Token.Read<SeperatorToken>,
        Token.Read<OperatorToken>,
        Token.Read<KeywordToken>,

        Token.Read<StringToken>,
        Token.Read<NumberToken>,
        Token.Read<IdentifierToken>,
    };

    private static void IgnoreSpaces(Pipe<char?> pipe, WriteLog log)
    {
        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext(log);

            if (!char.IsWhiteSpace(@char!.Value))
            {
                pipe.Position--;
                break;
            }
        }
    }

    public Token? Execute(Pipe<char?> pipe, WriteLog log) => Execute(pipe, log, false);

    public Token? Execute(Pipe<char?> pipe, WriteLog log, bool calledByExecute)
    {
        IgnoreSpaces(pipe, log);

        var fallback = pipe.Position;

        if (pipe.HasNext)
        {
            foreach (var reader in readers)
            {
                var token = reader(pipe, log);

                if (token != null)
                {
                    if (token.Length == 0)
                    {
                        continue;
                    }

                    IgnoreSpaces(pipe, log);
                    return token;
                }

                pipe.Position = fallback;
            }

            if (!calledByExecute)
            {
                var invalidTokenStart = pipe.Position;

                while (pipe.HasNext)
                {
                    var oldPosition = pipe.Position;
                    var expand = Execute(pipe, log, true) is null;

                    if (expand)
                    {
                        pipe.Position = oldPosition + 1;
                    }
                    else
                    {
                        pipe.Position = oldPosition;
                        break;
                    }
                }

                return new InvalidToken(invalidTokenStart, pipe.Position);
            }
        }

        return null;
    }
}