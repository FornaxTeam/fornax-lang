using System;
using System.IO;
using System.Linq;

namespace Fornax.Wasm.BaseNodes;

public class StaticData : ValueBasedBinaryNode
{
    public byte[] Data { get; }

    public override long Length => Data.LongLength;

    public override void Read(NodeReader reader)
    {
        if (!reader.ReadBuffer(Data.Length).SequenceEqual(Data))
            throw new BinaryNodeReadException();
    }

    public override void Reset()
    {
    }

    public override void Write(Stream stream) => stream.Write(Data);

    public StaticData(params byte[] data) => Data = data;
}