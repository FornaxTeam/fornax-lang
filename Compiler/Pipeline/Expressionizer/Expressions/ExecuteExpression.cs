using System;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public class ExecuteExpression : Expression
{
    public string Name { get; private set; } = "";

    public List<string> Args { get; } = new();
}