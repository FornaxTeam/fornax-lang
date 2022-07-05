using Fornax.Wasm.BaseNodes;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection.Metadata;

namespace Fornax.Wasm;

public sealed class NodeReader : IDisposable
{
    private readonly Stream stream;

    public NodeReader(Stream stream) => this.stream = stream;

    public NodeReader(string path) : this(new FileStream(path, FileMode.Open))
    {
    }

    public void Fallback(Func<bool> action)
    {
        var position = stream.Position;

        if (!action())
        {
            stream.Position = position;
        }
    }

    public bool HasData => ByteCountToRead > 0;

    public long ByteCountToRead => stream.Length - stream.Position;

    public T? Read<T>() where T : class, IBinaryNode, new()
    {
        T? result = null;

        Fallback(() =>
        {
            T? binaryNode = new();
            binaryNode.Reset();
            binaryNode.Position = stream.Position;

            try
            {
                binaryNode.Read(this);
                result = binaryNode;
                return true;
            }
            catch (BinaryNodeReadException)
            {
                return false;
            }
        });

        return result;
    }

    public byte[] ReadBuffer(int length)
    {
        var buffer = new byte[length];
        return buffer[..stream.Read(buffer, 0, length)];
    }

    // Read a single byte from the stream.
    public int ReadByte() => stream.ReadByte();

    public void Dispose()
    {
        stream.Dispose();
        GC.SuppressFinalize(this);
    }
}