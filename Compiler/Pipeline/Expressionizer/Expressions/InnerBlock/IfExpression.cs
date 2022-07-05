using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock.Constants;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock;

public record IfExpression(long Start, long End, IValueExpression? Condition, BlockExpression? IfBody, BlockExpression? ElseBody) : Expression(Start, End), IValueExpression, ICommandExpression
{
    public override string ToString() => base.ToString();

    public static IfExpression Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        IValueExpression? condition = null;
        BlockExpression? ifExpression = null, elseExpression = null;



        ParserFragment.Create()
            .Expect<KeywordToken>()
                .Where(token => token.Type == KeywordType.If)
                .MessageIfMissing("If keyword expected")
            .Call(StringConstant.Read)
                .Handle(constant => condition = constant)
                .Ok()
            .Call(BlockExpression.Read)
                .Handle(expression => ifExpression = expression)
            .Ok()
            .Call(ReadElseBlock)
                .Handle(expression => elseExpression = expression)
                .Ok()
            .Parse(pipe, log);

        return new(start, pipe.Position, condition, ifExpression, elseExpression);
    }

    private static BlockExpression? ReadElseBlock(Pipe<Token> pipe, WriteLog log)
    {
        BlockExpression? elseExpression = null;

        ParserFragment.Create()
            .Expect<KeywordToken>()
                .Where(token => token.Type == KeywordType.If)
                .MessageIfMissing("Else keyword expected")
            .Call(BlockExpression.Read)
                .Handle(expression => elseExpression = expression)
                .Ok()
            .Parse(pipe, log);

        return elseExpression;
    }
}

