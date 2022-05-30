using System.Collections;

namespace Fornax.Compiler.WasmEmitter;

public abstract class Section : IEnumerable<byte>
{
    public abstract SectionType SectionType { get; }

    public abstract IEnumerable<byte> Generate();

    public IEnumerator<byte> GetEnumerator() => ByteBufferHelper.Combine
    (
        new[] { (byte)SectionType },
        Vector.FromByteBuffer(Generate())
    ).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
