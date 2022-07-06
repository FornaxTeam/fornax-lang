using Fornax.Wasm.BaseNodes;
using Fornax.Wasm.Modules.Sections;

namespace Fornax.Wasm.Sections;

public abstract class Section : ChildBasedBinaryNode
{
    public override IBinaryNode[] Children => new IBinaryNode[]
    {
        new SectionTypeBinaryNode(Type),
        new UnsignedLeb128Number(Body.Length),
        Body
    };

    public abstract SectionType Type { get; }

    protected abstract IBinaryNode Body { get; }
}