using Fornax.Compiler.Logging;
using System;
using System.Linq;
using System.Text;

namespace Fornax.Compiler.Pipeline.Expressionizer;

public record Expression(long Start, long End)
{
    public long Length => End - Start;

    public override string ToString() => ObjectToStringConverter.ToString(this);
}