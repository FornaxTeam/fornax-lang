using System;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record CallExpression(long Start, long End, string Name) : Expression(Start, End);
