using Fornax.Wasm.BaseNodes;
using System.Text;

namespace Fornax.Wasm.Sections;

public sealed class CustomSection : Section
{
    public override SectionType Type => SectionType.Custom;

    protected override IBinaryNode Body => new StaticSizedBuffer(Encoding.ASCII.GetBytes("Hello World!"));
}