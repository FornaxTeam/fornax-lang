using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions;
using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler.Pipeline.Expressionizer;

public class ExpressionizerStep : IPipeStep<Token, Expression>
{
    public Expression? Execute(Pipe<Token> pipe, WriteLog log)
    {
        Expression? result = null;

        ParserFragment.Create()
            .Call(MethodExpression.Read)
                .Handle(document => result = document)
                .Ok()
            .Parse(pipe, log);

        return result;
    }
}
