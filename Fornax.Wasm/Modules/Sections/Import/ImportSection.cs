using Fornax.Wasm.BaseNodes;
using Fornax.Wasm.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Wasm.Modules.Sections.Import;

public class ImportSection : Section
{
    private readonly ImportSectionBody body = new();

    private class ImportSectionBody : ChildBasedBinaryNode
    {
        // TODO: Implementieren!!!
        public override IEnumerable<IBinaryNode> Children => throw new NotImplementedException();
    }

    public override SectionType Type => SectionType.Import;

    protected override IBinaryNode Body => body;
}