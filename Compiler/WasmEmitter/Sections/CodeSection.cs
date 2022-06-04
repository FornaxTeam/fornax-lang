using System.Collections.Generic;

namespace Fornax.Compiler.WasmEmitter.Sections;

public class CodeSection : Section
{
    public override SectionType SectionType => SectionType.Code;

    public List<FunctionBody> FunctionBodyList { get; } = new();

    public override IEnumerable<byte> Generate() => Vector.FromByteBuffer(FunctionBodyList.Count, ByteBufferHelper.Combine(FunctionBodyList));

    public FunctionBody Create()
    {
        FunctionBody body = new();
        FunctionBodyList.Add(body);
        return body;
    }
}
