using Fornax.Wasm.BaseNodes;
using Fornax.Wasm.Sections;
using System.IO;

namespace Fornax.Wasm.Modules.Sections;

public sealed class SectionTypeBinaryNode : ValueBasedBinaryNode
{
    public SectionType SectionType { get; }

    public override long Length => 1;

    public SectionTypeBinaryNode(SectionType sectionType) => SectionType = sectionType;

    public override void Read(NodeReader reader)
    {
        if ((byte)SectionType != reader.ReadByte())
            throw new BinaryNodeReadException();
    }

    public override void Reset()
    {
    }

    public override void Write(Stream stream) => stream.WriteByte((byte)SectionType);
}