using Fornax.Wasm.BaseNodes;
using System.IO;

namespace Fornax.Wasm.Module;

public sealed class ModuleVersion : ValueBasedBinaryNode
{
    public byte[] Data { get; private set; } = new byte[] { 1, 0, 0, 0 };

    public override long Length => 4;

    public override void Read(NodeReader reader)
    {
        var buffer = reader.ReadBuffer(4);

        if (buffer.Length != 4)
            throw new BinaryNodeReadException();

        Data = buffer;
    }

    public override void Reset() => Data = new byte[] { 1, 0, 0, 0 };

    public override void Write(Stream stream) => stream.Write(Data);
}