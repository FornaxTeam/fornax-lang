﻿using System.Text;

namespace Fornax.Wasm;

public sealed class CustomSection : Section
{
    public override SectionType Type => SectionType.Custom;

    protected override IBinaryNode Body => new StaticSizedBuffer(Encoding.ASCII.GetBytes("Hello World!"));
}