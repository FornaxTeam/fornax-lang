using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record ArgumentExpression(long Start, long End, string Type, string Name) : Expression(Start, End)
{
    public static ArgumentExpression Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        var type = "";
        var name = "";

        if (pipe.Fallback(fallbackPosition =>
        {
            return ParserFragment.Create()
                .Expect<IdentifierToken>()
                .Handle(token => type = token.Value)
                    .MessageIfMissing("Type expected.")
                .Expect<WhitespaceToken>()
                    .MessageIfMissing("Whitespace expected.")
                .Expect<IdentifierToken>()
                    .Handle(token => name = token.Value)
                    .MessageIfMissing("Parameter name expected.")
                .Parse(pipe, null);
        }))
        {
            return new(start, pipe.Position, type, name);
        }

        ParserFragment.Create()
            .Expect<IdentifierToken>()
                .Handle(token => name = token.Value)
                .MessageIfMissing("Parameter name expected.")
            .Parse(pipe, log);

        return new(start, pipe.Position, "", name);
    }
}
