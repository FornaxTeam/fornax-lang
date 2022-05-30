using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;

public enum OperatorType
{
    Assign,

    Add, Subtract, Multiply, Divide, Modulo,
    AssignAdd, AssignSubtract, AssignMultiply, AssignDivide, AssignModulo,

    AddOne, SubtractOne,
    EqualTo, NotEqualTo, GreaterThan, LessThan, GreaterThanOrEqualTo, LessThanOrEqualTo,

    And, Or, Xor, ShiftLeft, ShiftRight, ToggleBits,
    ConditionalAnd, ConditionalOr,

    Not,
}
