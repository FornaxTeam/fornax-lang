using System.Collections.Generic;
using System.IO;

namespace Fornax.Wasm.BaseNodes;

public interface IBinaryNode
{
    long Length { get; }

    long Position { get; set; }

    IEnumerable<IBinaryNode> Children { get; }

    void Read(NodeReader reader);

    void Reset();

    void Write(Stream stream);
}