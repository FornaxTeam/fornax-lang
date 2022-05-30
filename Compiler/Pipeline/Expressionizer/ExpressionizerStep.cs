using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using System;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer;

public class ExpressionizerStep : IPipeStep<Token, Expression>
{
    public Expression? Execute(Pipe<Token> pipe) => null;
}
