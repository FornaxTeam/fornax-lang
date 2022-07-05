using System;

namespace Fornax.Wasm;

public sealed class Module : ChildBasedBinaryNode
{
    public TypeSection? TypeSection { get; set; } = null;

    public override IBinaryNode[] Children => new IBinaryNode[]
    {
        new MagicValue(),
        new ModuleVersion(),
        Optional(() => TypeSection, value => TypeSection = value),
    };

    public static OptionalBinaryNode<T> Optional<T>(Func<T?> get, Action<T?> set) where T : class, IBinaryNode, new()
        => new(new(get, set));
}