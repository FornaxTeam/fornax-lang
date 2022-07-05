using System;

namespace Fornax.Wasm;

public sealed class Reference<T>
{
    private readonly Func<T?> get;
    private readonly Action<T?> set;

    public Reference(Func<T?> get, Action<T?> set)
    {
        this.get = get;
        this.set = set;
    }

    public T? Value
    {
        get => get();
        set => set(value);
    }
}