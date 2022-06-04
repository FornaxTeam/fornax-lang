using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.WasmEmitter;

public class FunctionBody : IEnumerable<byte>
{
    private readonly List<byte> data = new();

    public int LocalCount { get; set; } = 0;

    public IEnumerator<byte> GetEnumerator()
    {
        var localCountBuffer = ByteBufferHelper.UnsignedLeb128(LocalCount).ToArray();

        return Vector.FromByteBuffer
        (
            localCountBuffer.Length + data.Count,
            ByteBufferHelper.Combine(localCountBuffer, data)
        ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Emit(OpCode code) => data.Add((byte)code);

    private void Emit(OpCode code, int value)
    {
        data.Add((byte)code);
        data.AddRange(ByteBufferHelper.UnsignedLeb128(value));
    }

    public void Call(FunctionReference function) => Emit(OpCode.Call, function.Id);

    public void Const(int value) => Emit(OpCode.I32Const, value);
}