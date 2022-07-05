using System;
using System.Collections.Generic;
using System.Linq;

namespace Fornax.Wasm;

public class FunctionType : ChildBasedBinaryNode
{
    public BaseType[] Parameters { get; set; } = Array.Empty<BaseType>();

    public BaseType[] Results { get; set; } = Array.Empty<BaseType>();

    public override IEnumerable<IBinaryNode> Childs
    {
        get
        {
            yield return new StaticData(0x60);

            UnsignedLeb128Number parameterCount = new(Parameters.Length);
            yield return parameterCount;

            StaticSizedBuffer parameterTypes = Parameters.Length == parameterCount.Value
                ? new(Parameters.Select(type => (byte)type).ToArray())
                : new((int)parameterCount.Value);
            yield return parameterTypes;
            Parameters = parameterTypes.Data.Select(type => (BaseType)type).ToArray();

            UnsignedLeb128Number resultCount = new(Results.Length);
            yield return resultCount;

            StaticSizedBuffer resultTypes = Results.Length == resultCount.Value
                ? new(Results.Select(type => (byte)type).ToArray())
                : new((int)resultCount.Value);
            yield return resultTypes;
            Results = resultTypes.Data.Select(type => (BaseType)type).ToArray();
        }
    }

    public FunctionType(BaseType[] parameters, BaseType[] results)
    {
        Parameters = parameters;
        Results = results;
    }
}