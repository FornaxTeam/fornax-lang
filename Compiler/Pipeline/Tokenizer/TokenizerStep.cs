using System;
using System.Collections.Generic;

using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Brackets;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Comparison;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Mark;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.MathOperation;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separator;
using System.Linq;
using System.Reflection;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public class TokenizerStep : IPipeStep<char?, Token>
{
    public Token? Execute(Pipe<char?> pipe)
    {
        var readers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.BaseType == typeof(Token))
            .Select(type => new Func<Pipe<char?>, Token?>(current => (Token?)type.GetMethod("Read")!.Invoke(null, new object?[] { current })))
            .ToList();

        var fallback = pipe.Position;

        foreach (var token in readers.Select(reader => reader(pipe)))
        {
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

        return null;
    }
}