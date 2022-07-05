using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fornax.Wasm;

public abstract class ValueBasedBinaryNode : IBinaryNode
{
    public abstract long Length { get; }

    public abstract void Read(NodeReader reader);

    public abstract void Reset();

    public abstract void Write(Stream stream);

    public long Position { get; set; }

    public IEnumerable<IBinaryNode> Children => Enumerable.Empty<IBinaryNode>();
}