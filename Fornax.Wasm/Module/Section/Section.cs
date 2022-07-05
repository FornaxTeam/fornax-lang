using Fornax.Wasm.BaseNodes;

namespace Fornax.Wasm.Section;

public abstract class Section : ChildBasedBinaryNode
{
    public override IBinaryNode[] Childs => new IBinaryNode[]
    {
        new SectionTypeBinaryNode(Type),
        new UnsignedLeb128Number(Body.Length),
        Body
    };

    public abstract SectionType Type { get; }

    protected abstract IBinaryNode Body { get; }
}