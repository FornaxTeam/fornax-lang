using Fornax.Compiler.Logging;
using System.Linq;
using System.Text.Json.Serialization;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;

public class SeparatorToken : Token
{
    private readonly (SeparatorType separatorType, char separatorString)[] operators = new[]
    {
        (SeparatorType.Command, ';'),
        (SeparatorType.Value, ','),

        (SeparatorType.Member, '.'),
        (SeparatorType.CollectionMember, ':'),

        (SeparatorType.ValueOpen, '('),
        (SeparatorType.ValueClose, ')'),

        (SeparatorType.CollectionOpen, '['),
        (SeparatorType.CollectionClose, ']'),

        (SeparatorType.BlockOpen, '{'),
        (SeparatorType.BlockClose, '}'),
    };

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SeparatorType Type { get; private set; }

    protected override bool Read(Pipe<char?> pipe, WriteLog log)
    {
        foreach (var (separatorType, separatorString) in operators)
        {
            if (pipe.Fallback(fallbackPosition => pipe.HasNext && pipe.ReadNext(log)!.Value == separatorString))
            {
                Type = separatorType;
                return true;
            }
        }

        return false;
    }
}
