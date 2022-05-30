using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Operators;

public class OperatorToken : Token
{
    private readonly (OperatorType operatorType, string operatorString)[] operators = new[]
    {
        (OperatorType.Assign, "="),

        (OperatorType.Add, "+"),
        (OperatorType.Subtract, "-"),
        (OperatorType.Multiply, "*"),
        (OperatorType.Divide, "/"),
        (OperatorType.Modulo, "%"),

        (OperatorType.AssignAdd, "+="),
        (OperatorType.AssignSubtract, "-="),
        (OperatorType.AssignMultiply, "*="),
        (OperatorType.AssignDivide, "/="),
        (OperatorType.AssignMultiply, "%="),

        (OperatorType.AddOne, "++"),
        (OperatorType.SubtractOne, "--"),

        (OperatorType.EqualTo, "=="),
        (OperatorType.NotEqualTo, "!="),
        (OperatorType.GreaterThan, ">"),
        (OperatorType.GreaterThanOrEqualTo, ">="),
        (OperatorType.LessThan, "<"),
        (OperatorType.LessThanOrEqualTo, "<="),

        (OperatorType.And, "&"),
        (OperatorType.Or, "|"),
        (OperatorType.Xor, "^"),
        (OperatorType.ShiftLeft, "<<"),
        (OperatorType.ShiftRight, ">>"),
        (OperatorType.ToggleBits, "~"),

        (OperatorType.ConditionalAnd, "&&"),
        (OperatorType.ConditionalOr, "||"),

        (OperatorType.Not, "!"),
    }.OrderBy(entry => 8 - entry.Item2.Length).ToArray();

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperatorType Type { get; private set; }

    protected override bool Read(Pipe<char?> pipe)
    {
        foreach (var (operatorType, operatorString) in operators)
        {
            if (pipe.Fallback(fallbackPosition =>
            {
                for (var i = 0; i < operatorString.Length; i++)
                {
                    if (!pipe.HasNext || pipe.ReadNext()!.Value != operatorString[i])
                    {
                        return false;
                    }
                }

                return true;
            }))
            {
                Type = operatorType;
                return true;
            }
        }

        return false;
    }
}
