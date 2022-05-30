using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fornax.Compiler.Pipeline.Tokenizer;

public abstract class Token
{
    public long Start { get; protected set; }

    [JsonIgnore]
    public long End { get; protected set; }

    public long Length => End - Start;

    protected abstract bool Read(Pipe<char?> pipe);

    public static T? Read<T>(Pipe<char?> pipe) where T : Token, new()
    {
        T result = new()
        {
            Start = pipe.Position
        };

        result.End = result.Read(pipe) ? pipe.Position : result.Start;
        return result.Start == result.End ? null : result;
    }

    public override string ToString() => GetType().Name + JsonSerializer.Serialize<object>(this);
}
