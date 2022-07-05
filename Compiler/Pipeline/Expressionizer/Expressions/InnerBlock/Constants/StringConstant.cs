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

public record StringConstant(long Start, long End, StringToken Token) : Expression(Start, End), IValueExpression
{
    public override string ToString() => base.ToString();

    public static StringConstant Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        StringToken? stringToken = null;

        ParserFragment.Create()
        .Expect<StringToken>()
            .Handle(token => stringToken = token)
            .MessageIfMissing("String token expected.")
        .Parse(pipe, log);

        return new(start, pipe.Position, stringToken!);
    }
}
