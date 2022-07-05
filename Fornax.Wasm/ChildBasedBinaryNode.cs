using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fornax.Wasm;

public abstract class ChildBasedBinaryNode : IBinaryNode
{
    public abstract IEnumerable<IBinaryNode> Childs { get; }

    public long Length => Childs.Sum(child => child.Length);

    public long Position { get; set; }

    public void Read(NodeReader reader)
    {
        foreach (var child in Childs)
        {
            child.Read(reader);
        }
    }

    public void Reset()
    {
        foreach (var child in Childs)
        {
            child.Reset();
        }
    }

    public void Write(Stream stream)
    {
        foreach (var child in Childs)
        {
            child.Write(stream);
        }
    }
}