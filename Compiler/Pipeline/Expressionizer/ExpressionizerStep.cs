using Fornax.Compiler.Pipeline.Expressionizer.Expressions;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using System;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer;

public class ExpressionizerStep : IPipeStep<Token, Expression>
{
    public Expression? Execute(Pipe<Token> pipe)
    {
        List<Func<Pipe<Token>, Expression?>> readers = new()
        {
            ReadMethod
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

    private MethodExpression? ReadMethod(Pipe<Token> pipe)
    {
        var start = pipe.Position;
        var export = false;

        var keyword = pipe.Expect<KeywordToken>()?.Keyword;

        if (keyword is not null)
        {
            if (keyword == Keyword.Export)
            {
                export = true;
            }
            else
            {
                return null;
            }
        }

        var name = pipe.Expect<IdentifierToken>()?.Name;

        return name is null ? null : new MethodExpression(start, pipe.Position, export);
    }
}
