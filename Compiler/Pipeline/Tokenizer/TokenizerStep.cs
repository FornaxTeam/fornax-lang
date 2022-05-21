using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public class TokenizerStep : IPipeStep<char?, Token>
{
    public Token? Execute(Pipe<char?> pipe)
    {
        List<Func<Pipe<char?>, Token?>> readers = new()
        {
            ReadArrow,
            ReadAssignments,
            ReadBrackets,
            ReadEndOfCommand,
            ReadKeyword,
            ReadIdentifier,
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
                pipe.Position -= 2;
                return false;
            }

            var multiLine = @char == '*';

            var firstStar = false;

            while (true)
            {
                @char = pipe.ReadNext();
                if (@char is '\n' or '\r' && !multiLine || multiLine && firstStar && @char == '/')
                {
                    break;
                }
                firstStar = @char == '*';
            }
        }
        else
        {
            pipe.Position--;
            return false;
        }

        return true;
    }

    private static AssignmentToken? ReadAssignments(Pipe<char?> pipe)
    {
        var content = pipe.ReadNext();

        return content == '=' ? new AssignmentToken(pipe.Position - 1, pipe.Position) : null;
    }
    
    private static ArrowToken? ReadArrow(Pipe<char?> pipe)
    {
        var content = pipe.ReadNext();

        if (content != '=') return null;
        content = pipe.ReadNext();
        return content == '>' ? new ArrowToken(pipe.Position - 2, pipe.Position) : null;
    }

    private static BracketToken? ReadBrackets(Pipe<char?> pipe)
    {
        var start = pipe.Position;

        if (!pipe.HasNext)
        {
            return null;
        }

        var @char = pipe.ReadNext()!.Value;

        var opened = new[] { '(', '{', '[' }.Contains(@char);
        Bracket bracket;

        switch (@char)
        {
            case '(':
            case ')':
                bracket = Bracket.Parameter;
                break;
            case '[':
            case ']':
                bracket = Bracket.Array;
                break;
            case '{':
            case '}':
                bracket = Bracket.Block;
                break;
            default:
                return null;
        }

        return new BracketToken(start, start + 1, bracket, opened);
    }

    private EndOfCommandToken? ReadEndOfCommand(Pipe<char?> pipe)
    {
        var start = pipe.Position;
        var @char = pipe.ReadNext();

        if (@char == ';')
        {
            return new EndOfCommandToken(start, start + 1);
        }

        return null;
    }

    private KeywordToken? ReadKeyword(Pipe<char?> pipe)
    {
        var identifier = ReadIdentifier(pipe);

        if (identifier == null || identifier.Length == 0)
        {
            return null;
        }

        var keyword = KeywordToken.GetKeyword(identifier.Name);

        return keyword.HasValue
            ? new KeywordToken(identifier.Start, identifier.End, keyword.Value)
            : null;
    }

    private static IdentifierToken? ReadIdentifier(Pipe<char?> pipe)
    {
        StringBuilder stringBuilder = new();
        var start = pipe.Position;

        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext()!.Value;

            if (!char.IsLetter(@char))
            {
                pipe.Position--;
                return new IdentifierToken(start, pipe.Position, stringBuilder.ToString());
            }

            stringBuilder.Append(@char);
        }

        return new IdentifierToken(start, pipe.Position, stringBuilder.ToString());
    }
}
