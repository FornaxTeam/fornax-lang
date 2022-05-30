using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.WasmEmitter.Sections;

public class StartSection : Section
{
    public override SectionType SectionType => SectionType.Start;

    public FunctionReference? Function { get; set; } = null;

    public override IEnumerable<byte> Generate() => ByteBufferHelper.UnsignedLeb128(Function?.Id ?? 0);
}
