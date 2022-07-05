using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fornax.Wasm;

public sealed class OptionalBinaryNode<T> : IBinaryNode, IHasBinaryNodeValue where T : class, IBinaryNode, new()
{
    private readonly Reference<T> reference;

    public T? Value
    {
        get => reference.Value;
        set => reference.Value = value;
    }

    IBinaryNode? IHasBinaryNodeValue.Value
    {
        get => Value;
        set => Value = (T?)value;
    }

    public long Length => Value?.Length ?? 0;

    public long Position { get; set; }

    public IEnumerable<IBinaryNode> Childs => Value is null ? Enumerable.Empty<IBinaryNode>() : new[] { Value };

    public OptionalBinaryNode(Reference<T> reference) => this.reference = reference;

    public void Read(NodeReader reader) => Value = reader.Read<T>();

    public void Reset() => Value = null;

    public void Write(Stream stream)
    {
        if (Value is not null)
        {
            Value.Write(stream);
        }
    }
}