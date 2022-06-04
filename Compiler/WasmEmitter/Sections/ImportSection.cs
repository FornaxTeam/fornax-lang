using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.WasmEmitter.Sections;

public class ImportSection : Section
{
    public record Import(string Module, string Name, int FunctionTypeId, int Kind = 0) : IEnumerable<byte>
    {
        public IEnumerator<byte> GetEnumerator() => ByteBufferHelper.Combine
        (
            ByteBufferHelper.EncodeString(Module),
            ByteBufferHelper.EncodeString(Name),
            ByteBufferHelper.UnsignedLeb128(Kind),
            ByteBufferHelper.UnsignedLeb128(FunctionTypeId)
        ).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public override SectionType SectionType => SectionType.Import;

    public List<Import> ImportList { get; } = new();

    public override IEnumerable<byte> Generate() => Vector.FromByteBuffer(ImportList.Count, ByteBufferHelper.Combine(ImportList));

    public FunctionReference Create(Module module, string externModuleName, string name, int functionTypeId)
    {
        ImportList.Add(new Import(externModuleName, name, functionTypeId));
        return new(module, ImportList.Count - 1, true);
    }
}
