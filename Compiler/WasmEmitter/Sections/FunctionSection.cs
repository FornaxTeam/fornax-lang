namespace Fornax.Compiler.WasmEmitter.Sections;

public class FunctionSection : Section
{
    public override SectionType SectionType => SectionType.Function;

    public List<int> TypeIndexes { get; } = new();

    public override IEnumerable<byte> Generate() => Vector.From(TypeIndexes
        .Select(type => ByteBufferHelper.UnsignedLeb128(type))
        .ToArray());

    public FunctionReference Create(Module module, int functionTypeId)
    {
        TypeIndexes.Add(functionTypeId);
        return new FunctionReference(module, TypeIndexes.Count - 1, false);
    }
}
