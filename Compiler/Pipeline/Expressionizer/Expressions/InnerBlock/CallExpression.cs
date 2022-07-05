using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock.Constants;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock;

public record CallExpression(long Start, long End, IdentifierToken? Name, IValueExpression[] Parameters) : Expression(Start, End), IValueExpression, ICommandExpression
{
    public override string ToString() => base.ToString();

    public static CallExpression Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        List<IValueExpression> parameters = new();
        IdentifierToken? name = null;

        ParserFragment.Create()
            .Expect<IdentifierToken>()
                .Handle(token => name = token)
                .MessageIfMissing("Name of method expected.")
            .Expect<SeparatorToken>()
                .Where(token => token.Type == SeparatorType.ValueOpen)
                .MessageIfMissing("'(' expected.")
            .Call(StringConstant.Read)
                .Handle(parameters.Add)
                .Ok()
            .Expect<SeparatorToken>()
                .Where(token => token.Type == SeparatorType.ValueClose)
                .MessageIfMissing("')' expected.")
            .Parse(pipe, log);

        return new(start, pipe.Position, name, parameters.ToArray());
    }

    public static CallExpression ReadAsCommand(Pipe<Token> pipe, WriteLog log)
    {
        CallExpression? result = null;

        ParserFragment.Create()
            .Call(Read)
                .Handle(expression => result = expression)
                .Ok()
            .Expect<SeparatorToken>()
                .Where(token => token.Type == SeparatorType.Command)
                .MessageIfMissing("';' expected.")
            .Parse(pipe, log);

        return result!;
    }
}

