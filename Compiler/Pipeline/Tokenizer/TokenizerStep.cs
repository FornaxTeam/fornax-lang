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
        RemoveWhitespaces(pipe);
        RemoveComments(pipe);
        RemoveWhitespaces(pipe);
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

    private static void RemoveComments(Pipe<char?> pipe)
    {
        // Remove comments
        var @char = pipe.ReadNext();

        if (@char == '/')
        {
            @char = pipe.ReadNext();

            if (@char != '/')
            {
                pipe.Position -= 2;
                return;
            }

            while (true)
            {
                @char = pipe.ReadNext();

                if (@char is '\n' or '\r')
                {
                    break;
                }
            }
        }
        else
        {
            pipe.Position--;
        }
    }

    /*
    private Token? ReadAssignments(Pipe<char> pipe)
    {
        var content = pipe.ReadNext();

        if (content == '=')
        {
            return new IToken(TokenType.ASSIGNMENT, content);
        }

        return null;
    }
    */

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
