using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fornax.Compiler.WasmEmitter;

public static class ByteBufferHelper
{
    public static IEnumerable<byte> Combine(params IEnumerable<byte>[] buffer) => buffer.SelectMany(buffer => buffer);

    public static IEnumerable<byte> Combine(IEnumerable<IEnumerable<byte>> buffer) => buffer.SelectMany(buffer => buffer);

    public static IEnumerable<byte> UnsignedLeb128(int value)
    {
        List<byte> result = new();

        do
        {
            var @byte = value & 0x7f;
            value >>= 7;

            if (value != 0)
            {
                @byte |= 0x80;
            }

            result.Add((byte)@byte);
        }
        while (value != 0);

        return result;
    }

    public static Vector EncodeString(string value) => Vector.FromByteBuffer(Encoding.ASCII.GetBytes(value));

    public static void Save(this IEnumerable<byte> source, string filePath)
    {
        using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write);

        foreach (var buffer in source.Chunk(1024 * 8))
        {
            fileStream.Write(buffer);
        }
    }

    public static void Print(this IEnumerable<byte> buffer, int index = -1)
    {
        foreach (var line in buffer
            .Chunk(32)
            .Select((buffer, chunkIndex) => string.Join(" ", buffer.Select((@byte, innerChunkIndex) =>
            {
                var offset = (chunkIndex * 32) + innerChunkIndex;
                return offset == index ? $"[{@byte:X2}]" : @byte.ToString("X2");
            }))))
        {
            Console.WriteLine(line);
        }
    }
}
