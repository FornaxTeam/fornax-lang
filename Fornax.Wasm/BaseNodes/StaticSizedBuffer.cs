using System;
using System.IO;

namespace Fornax.Wasm.BaseNodes;

public sealed class StaticSizedBuffer : ValueBasedBinaryNode
{
    public byte[] Data { get; private set; }

    public override long Length => Data.Length;

    public StaticSizedBuffer(int length) => Data = new byte[length];

    public StaticSizedBuffer(byte[] data) => Data = data;

    public override void Read(NodeReader reader)
    {
        var buffer = reader.ReadBuffer(Data.Length);

        if (buffer.Length != Data.Length)
            throw new BinaryNodeReadException();

        Data = buffer;
    }

    public override void Reset() => Data = Array.Empty<byte>();

    public override void Write(Stream stream) => stream.Write(Data);
}