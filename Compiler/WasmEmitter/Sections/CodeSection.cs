using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
