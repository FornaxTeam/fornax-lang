using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public class TokenizerStep : IPipeStep<char, Token>
{
    private static readonly string[] keywords = { "import", "export", "if", "else", "loop", "while", "for", "in", "var", "final", "const", "break", "test", "continue", "struct", "extension", "return", "is", "as" };
    private static readonly string[] brackets = { "(", ")", "{", "}", "[", "]" };

    private static readonly Dictionary<TokenType, Type> x = new()
    {
        [TokenType.CHAR] = typeof(char)
    };

    public Token? Execute(Pipe<char> pipe)
    {
        List<Func<Pipe<char>, Token?>> readers = new()
        {
            ReadAssignments,
            ReadBrackets,
            ReadKeyword,
            ReadIdentifier
        };

        var fallback = pipe.Position;

        foreach (var reader in readers)
        {
            var token = reader(pipe);

            if (token != null)
            {
                return token;
            }

            pipe.Position = fallback;
        }

        return null;
    }

    private Token? ReadAssignments(Pipe<char> pipe)
    {
        var content = pipe.ReadNext();
        
        if(content == '=')
        {
            return new Token(TokenType.ASSIGNMENT, content);
        }
        return null;
    }

    private Token? ReadBrackets(Pipe<char> pipe)
    {
        var content = pipe.ReadNext();

        if (brackets.Contains(pipe))
        {
            return new Token<bool>(TokenType.BRACKET, BrackType.Open);
        }

        return null;
    }

    private Token? ReadKeyword(Pipe<char> pipe)
    {
        var identifier = ReadIdentifier(pipe);

        if (identifier == null)
        {
            return null;
        }

        if (keywords.Contains(identifier.Value))
        {
            return new Token(TokenType.KEYWORD, identifier.Value);
        }

        return null;
    }

    private Token? ReadIdentifier(Pipe<char> pipe)
    {
        var content = pipe.ReadNext();

        while (current != ' ' || current != null)
        {
            content += current;
            current = pipe.ReadNext();
        }

        return new Token?(TokenType.IDENTIFIER, content);
    }
}