using System.Text.Json;

namespace Fornax.Compiler.Pipeline.Expressionizer;

public record Expression(long Start, long End)
{
    public long Length => End - Start;

    public override string ToString() => GetType().Name + JsonSerializer.Serialize<object>(this, new JsonSerializerOptions()
    {
        WriteIndented = true
    });
}