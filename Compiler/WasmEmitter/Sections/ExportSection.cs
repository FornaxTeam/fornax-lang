using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.WasmEmitter.Sections;

public class ExportSection : Section
{
    public record Export(string Name, FunctionReference Function, int Kind = 0) : IEnumerable<byte>
    {
        public IEnumerator<byte> GetEnumerator() => ByteBufferHelper.Combine
        (
            ByteBufferHelper.EncodeString(Name),
            ByteBufferHelper.UnsignedLeb128(Kind),
            ByteBufferHelper.UnsignedLeb128(Function.Id)
        ).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public override SectionType SectionType => SectionType.Export;

    public List<Export> ExportList { get; } = new();

    public override IEnumerable<byte> Generate() => Vector.FromByteBuffer(ExportList.Count, ByteBufferHelper.Combine(ExportList));

    public void Create(string name, FunctionReference function) => ExportList.Add(new Export(name, function));
}
