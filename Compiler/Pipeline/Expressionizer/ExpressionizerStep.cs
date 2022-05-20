using Fornax.Compiler.Pipeline.Tokenizer;
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

    private Expression? ReadMethod(Pipe<Token> pipe)
    {
        var token = pipe.ReadNext();
        var export = false;

        if (token is KeywordToken keywordToken && keywordToken.Keyword == Keyword.Export)
        {
            export = true;
        }

        return null;
    }
}
