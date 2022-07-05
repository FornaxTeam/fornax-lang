namespace Fornax.Wasm;

public sealed class MagicValue : StaticData
{
    public MagicValue() : base(0, 0x61, 0x73, 0x6d)
    {
    }
}