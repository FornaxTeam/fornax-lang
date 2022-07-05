using Fornax.Wasm.BaseNodes;
using Fornax.Wasm.Sections;
using Fornax.Wasm.Sections.Type;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Wasm.Sections;

public sealed class TypeSection : Section
{
    private readonly TypeSectionBody body = new();

    private class TypeSectionBody : ChildBasedBinaryNode
    {
        public List<FunctionType> Types { get; set; } = new();

        public override IEnumerable<IBinaryNode> Children
        {
            get
            {
                UnsignedLeb128Number typeCount = new(Types.Count);
                yield return typeCount;

                var types = Types.Count == typeCount.Value
                    ? Types.ToArray()
                    : new FunctionType[typeCount.Value];

                for (var i = 0; i < types.Length; i++)
                    yield return types[i] ??= new FunctionType(Array.Empty<BaseType>(), Array.Empty<BaseType>());

                Types = types.ToList();
            }
        }
    }

    public override SectionType Type => SectionType.Type;

    protected override IBinaryNode Body => body;

    public List<FunctionType> Types
    {
        get => body.Types;
        set => body.Types = value;
    }
}