using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using System;
using System.Collections.Generic;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Comparison;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Mark;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.MathOperation;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separator;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public class TokenizerStep : IPipeStep<char?, Token>
{
    public Token? Execute(Pipe<char?> pipe)
    {
        List<Func<Pipe<char?>, Token?>> readers = new()
        {
            ComparisonToken.Read,
            ArrowToken.Read,
            AssignmentToken.Read,
            BracketToken.Read,
            StringToken.Read,
            NumberToken.Read,
            MarkToken.Read,
            MathOperationToken.Read,
            SeparatorToken.Read,
            EndOfCommandToken.Read,
            SeparatorToken.Read,
            KeywordToken.Read,
            IdentifierToken.Read
        };

        RemoveEmptyAreas(pipe);

        var fallback = pipe.Position;

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

        RemoveEmptyAreas(pipe);

        return null;
    }

    private static void RemoveEmptyAreas(Pipe<char?> pipe)
    {
        var comments = true;

        while (comments)
        {
            RemoveWhitespaces(pipe);
            comments = RemoveComment(pipe);
            RemoveWhitespaces(pipe);
        }
    }

    private static void RemoveWhitespaces(Pipe<char?> pipe)
    {
        while (true)
        {
            var @char = pipe.ReadNext();

            if (@char == null)
            {
                return;
            }

            if (!char.IsWhiteSpace(@char.Value))
            {
                pipe.Position--;
                break;
            }
        }
    }

    private static bool RemoveComment(Pipe<char?> pipe)
    {
        var @char = pipe.ReadNext();

        if (@char == '/')
        {
            @char = pipe.ReadNext();

            if (@char is not ('/' or '*'))
            {
                if (pipe.HasNext)
                {
                    pipe.Position -= 2;
                }
                else
                {
                    pipe.Position--;
                }

                return false;
            }

            var multiLine = @char == '*';

            var firstStar = false;

            while (true)
            {
                @char = pipe.ReadNext();

                if ((@char is '\n' or '\r' && !multiLine)
                    || (multiLine && firstStar && @char == '/'))
                {
                    break;
                }

                firstStar = @char == '*';
            }
        }
        else
        {
            if (@char != null)
            {
                pipe.Position--;
            }

            return false;
        }

        return true;
    }
}