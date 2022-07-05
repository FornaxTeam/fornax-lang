using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fornax.Wasm.BaseNodes;

public abstract class ChildBasedBinaryNode : IBinaryNode
{
    public abstract IEnumerable<IBinaryNode> Children { get; }

    public long Length => Children.Sum(child => child.Length);

    public long Position { get; set; }

    public void Read(NodeReader reader)
    {
        foreach (var child in Children)
        {
            child.Read(reader);
        }
    }

    public void Reset()
    {
        foreach (var child in Children)
        {
            child.Reset();
        }
    }

    public void Write(Stream stream)
    {
        foreach (var child in Children)
        {
            child.Write(stream);
        }
    }
}