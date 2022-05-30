using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Compiler.WasmEmitter.Sections;

namespace Fornax.Compiler.WasmEmitter;

public class Module : IEnumerable<byte>
{
    private readonly StartSection start;

    public static NumberType[] EmptyTypes { get; } = Array.Empty<NumberType>();

    public TypeSection Types { get; }

    public ImportSection Imports { get; }

    public FunctionSection Functions { get; }

    public ExportSection Exports { get; }

    public CodeSection FunctionBodies { get; }

    public Module()
    {
        Types = new();
        Imports = new();
        Functions = new();
        Exports = new();
        start = new();
        FunctionBodies = new();
    }

    public IEnumerator<byte> GetEnumerator() => ByteBufferHelper.Combine
    (
        new byte[] { 0x00, 0x61, 0x73, 0x6d },
        new byte[] { 1, 0, 0, 0 },
        Types,
        Imports,
        Functions,
        Exports,
        start.Function is null ? Enumerable.Empty<byte>() : start,
        FunctionBodies
    ).GetEnumerator();

    public FunctionReference CreateFunction(NumberType[]? parameters, NumberType[]? result, Action<FunctionBody> createBody)
    {
        if (parameters is null)
        {
            parameters = EmptyTypes;
        }

        if (result is null)
        {
            result = EmptyTypes;
        }

        var typeId = Types.Create(parameters, result);
        var function = Functions.Create(this, typeId);

        var body = FunctionBodies.Create();
        createBody(body);
        body.Emit(OpCode.End);

        return function;
    }

    public FunctionReference Import(string moduleName, string name, NumberType[]? parameters = null, NumberType[]? result = null)
    {
        if (parameters is null)
        {
            parameters = EmptyTypes;
        }

        if (result is null)
        {
            result = EmptyTypes;
        }

        var importTypeId = Types.Create(parameters, result);
        return Imports.Create(this, moduleName, name, importTypeId);
    }

    public FunctionReference? Main
    {
        get => start.Function;
        set => start.Function = value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
