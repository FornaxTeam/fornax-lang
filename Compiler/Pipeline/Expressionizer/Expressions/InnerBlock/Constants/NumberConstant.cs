using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Literals;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock.Constants;

public record NumberConstant(long Start, long End, NumberToken? Token) : Expression(Start, End), IValueExpression
{
    public override string ToString() => base.ToString();

    public static NumberConstant Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        NumberToken? numberToken = null;

        ParserFragment.Create()
            .Expect<NumberToken>()
                .Handle(token => numberToken = token)
                .MessageIfMissing("Number token expected.")
            .Parse(pipe, log);

        return new(start, pipe.Position, numberToken);
    }
}