using System.Collections;

namespace Fornax.Compiler.WasmEmitter;

public class Vector : IEnumerable<byte>
{
    private int length = 0;
    private IEnumerable<byte> data = Enumerable.Empty<byte>();

    private Vector() { }

    public static Vector FromByteBuffer(IEnumerable<byte> buffer)
    {
        var list = buffer.ToList();

        return new()
        {
            data = list,
            length = list.Count
        };
    }

    public static Vector FromByteBuffer(int length, IEnumerable<byte> buffer)
    {
        return new()
        {
            data = buffer,
            length = length
        };
    }

    public static Vector From(params IEnumerable<byte>[] buffers)
    {
        return new()
        {
            data = ByteBufferHelper.Combine(buffers),
            length = buffers.Length
        };
    }

    public IEnumerator<byte> GetEnumerator() => ByteBufferHelper.Combine
    (
        ByteBufferHelper.UnsignedLeb128(length),
        data
    ).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
