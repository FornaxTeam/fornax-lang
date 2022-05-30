using System.Linq;
using System.Text.Json.Serialization;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Seperators;

public class SeperatorToken : Token
{
    private readonly (SeperatorType seperatorType, char seperatorString)[] operators = new[]
    {
        (SeperatorType.Command, ';'),
        (SeperatorType.Value, ','),

        (SeperatorType.Member, '.'),
        (SeperatorType.CollectionMember, ':'),

        (SeperatorType.ValueOpen, '('),
        (SeperatorType.ValueClose, ')'),

        (SeperatorType.CollectionOpen, '['),
        (SeperatorType.CollectionClose, ']'),

        (SeperatorType.BlockOpen, '{'),
        (SeperatorType.BlockClose, '}'),
    };

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SeperatorType Type { get; private set; }

    protected override bool Read(Pipe<char?> pipe)
    {
        foreach (var (seperatorType, seperatorString) in operators)
        {
            if (pipe.Fallback(fallbackPosition => pipe.HasNext && pipe.ReadNext()!.Value == seperatorString))
            {
                Type = seperatorType;
                return true;
            }
        }

        return false;
    }
}
