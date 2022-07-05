using System.Collections.Generic;
using System.IO;

namespace Fornax.Wasm;

public interface IBinaryNode
{
    long Length { get; }

    long Position { get; set; }

    IEnumerable<IBinaryNode> Childs { get; }

    void Read(NodeReader reader);

    void Reset();

    void Write(Stream stream);
}