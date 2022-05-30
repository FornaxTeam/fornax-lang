using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Literals;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Seperators;
using System;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public class TokenizerStep : IPipeStep<char?, Token>
{
    private static readonly Func<Pipe<char?>, Token?>[] readers = new Func<Pipe<char?>, Token?>[]
    {
        Token.Read<SpaceToken>,

        Token.Read<SeperatorToken>,
        Token.Read<OperatorToken>,
        Token.Read<KeywordToken>,

        Token.Read<StringToken>,
        Token.Read<NumberToken>,
        Token.Read<IdentifierToken>,
    };

    public Token? Execute(Pipe<char?> pipe) => Execute(pipe, false);

    public Token? Execute(Pipe<char?> pipe, bool calledByExecute)
    {
        var fallback = pipe.Position;

        if (pipe.HasNext)
        {
            foreach (var reader in readers)
            {
                var token = reader(pipe);

                if (token != null)
                {
                    if (token.Length == 0)
                    {
                        continue;
                    }

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
                    var expand = Execute(pipe, true) is null;

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