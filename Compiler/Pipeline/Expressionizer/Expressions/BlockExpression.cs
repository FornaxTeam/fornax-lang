using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Seperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record BlockExpression(long Start, long End) : Expression(Start, End)
{
    public static BlockExpression? Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;

        ParserFragment.Create()
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<SeperatorToken>()
                .Where(token => token.Type == SeperatorType.BlockOpen)
                .MessageIfMissing("'{' expected.")
            .Expect<WhitespaceToken>()
                .Optional()
            // Content
            .Expect<WhitespaceToken>()
                .Optional()
            .Expect<SeperatorToken>()
                .Where(token => token.Type == SeperatorType.BlockClose)
                .MessageIfMissing("'}' expected.")
            .Expect<WhitespaceToken>()
                .Optional()
            .Parse(pipe, log);

        return new(start, pipe.Position);
    }
}
