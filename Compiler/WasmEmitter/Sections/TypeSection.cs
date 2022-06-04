using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.WasmEmitter.Sections;

public class TypeSection : Section
{
    public class FunctionType : IEnumerable<byte>
    {
        public NumberType[] Parameters { get; set; } = Array.Empty<NumberType>();

        public NumberType[] Results { get; set; } = Array.Empty<NumberType>();

        public IEnumerator<byte> GetEnumerator() => ByteBufferHelper.Combine
        (
            new[] { (byte)0x60 },
            Vector.FromByteBuffer(Parameters.Length, Parameters.Select(param => (byte)param)),
            Vector.FromByteBuffer(Results.Length, Results.Select(result => (byte)result))
        ).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public override SectionType SectionType => SectionType.Type;

    public List<FunctionType> Types { get; } = new();

    public override IEnumerable<byte> Generate() => Vector.FromByteBuffer(Types.Count, ByteBufferHelper.Combine(Types));

    public int Create(NumberType[] parameters, NumberType[] results)
    {
        for (var i = 0; i < Types.Count; i++)
        {
            var type = Types[i];

            if (parameters.Length == type.Parameters.Length && results.Length == type.Results.Length
                && parameters
                    .Select((param, index) => (param, index))
                    .All(entry => type.Parameters[entry.index] == entry.param)
                && results
                    .Select((result, index) => (result, index))
                    .All(entry => type.Parameters[entry.index] == entry.result))
            {
                return i;
            }
        }

        Types.Add(new FunctionType()
        {
            Parameters = parameters,
            Results = results
        });

        return Types.Count - 1;
    }
}
