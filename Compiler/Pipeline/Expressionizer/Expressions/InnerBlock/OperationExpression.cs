using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock.Constants;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock;

public record OperationExpression(long Start, long End, IValueExpression? FirstExpression, IValueExpression? SecondExpression, OperatorType Operator) : Expression(Start, End), IValueExpression, ICommandExpression
{
    public override string ToString() => base.ToString();

    public static OperationExpression Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        IValueExpression? first = null, second = null;
        OperatorType @operator = 0;

        ParserFragment.Create()
            .Call(StringConstant.Read)
                .Handle(stringConstant => first = stringConstant)
                .Ok()
            .Expect<OperatorToken>()
                .Handle(operatorToken => @operator = operatorToken.Type)
                .MessageIfMissing("Operator expected.")
            .Call(StringConstant.Read)
                .Handle(stringConstant => second = stringConstant)
                .Ok()
            .Parse(pipe, log);

        return new(start, pipe.Position, first, second, @operator);
    }
}

