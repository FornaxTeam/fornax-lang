using System.IO;

namespace Fornax.Wasm.BaseNodes;

public sealed class UnsignedLeb128Number : ValueBasedBinaryNode
{
    public long Value { get; set; }

    public UnsignedLeb128Number() => Value = 0;

    public UnsignedLeb128Number(long value) => Value = value;

    // Get the length of the number in bytes.
    public override long Length
    {
        get
        {
            var length = 0;
            var value = Value;

            do
            {
                value >>= 7;
                length++;
            }
            while (value != 0);

            return length;
        }
    }

    public override void Read(NodeReader reader)
    {
        var result = 0;
        var shift = 0;

        while (true)
        {
            var @byte = reader.ReadByte();

            if (@byte == -1)
                throw new BinaryNodeReadException();

            result |= (@byte & 0x7f) << shift;

            if ((@byte & 0x80) == 0)
                break;

            shift += 7;
        }

        Value = result;
    }

    public override void Reset() => Value = 0;

    public override void Write(Stream stream)
    {
        var value = Value;

        do
        {
            var @byte = value & 0x7f;
            value >>= 7;

            if (value != 0)
                @byte |= 0x80;

            stream.WriteByte((byte)@byte);
        }
        while (value != 0);
    }
}