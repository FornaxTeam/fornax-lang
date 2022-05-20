using Fornax.Compiler.Pipeline.Expressionizer.Expressions;
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

    private static bool ReadExport(Pipe<Token> pipe)
    {
        var export = false;

        pipe.Fallback(() =>
        {
            var token = pipe.ReadNext();

            if (token is KeywordToken keywordToken && keywordToken.Keyword == Keyword.Export)
            {
                export = true;
                return true;
            }

            return false;
        });

        return export;
    }

    private Expression? ReadMethod(Pipe<Token> pipe)
    {
        var start = pipe.Position;
        var export = ReadExport(pipe);

        return new MethodExpression(start, pipe.Position, export);
    }
}
