namespace Fornax.Compiler.WasmEmitter;

public class FunctionReference
{
    private readonly int index;

    public Module Module { get; private set; }

    public int Id => index + (IsFromImport ? 0 : Module.Imports.ImportList.Count);

    public bool IsFromImport { get; private set; }

    public FunctionReference(Module module, int index, bool isFromImport)
    {
        Module = module;
        this.index = index;
        IsFromImport = isFromImport;
    }
}
