using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record ArgumentExpression(long Start, long End, string Type, string Name) : Expression(Start, End)
{
    public static (string type, string name) Read(Pipe<Token> pipe, WriteLog log)
    {
        var type = "";
        var name = "";

        if (pipe.Fallback(fallbackPosition =>
            {
                return ParserFragment.Create()
                    .Expect<IdentifierToken>()
                    .Handle(token => type = token.Value)
                    .MessageIfMissing("Type expected.")
                    .Expect<SpaceToken>()
                    .MessageIfMissing("Whitespace expected.")
                    .Expect<IdentifierToken>()
                    .Handle(token => name = token.Value)
                    .MessageIfMissing("Parameter name expected.")
                    .Parse(pipe, null);
            })) return (type, name);
        {
            type = "";

            ParserFragment.Create()
                .Expect<IdentifierToken>()
                .Handle(token => name = token.Value)
                .MessageIfMissing("Parameter name expected.")
                .Parse(pipe, log);
        }

        return (type, name);
    }
}
