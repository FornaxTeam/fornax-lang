using Fornax.Wasm.BaseNodes;

namespace Fornax.Wasm;

public interface IHasBinaryNodeValue
{
    IBinaryNode? Value { get; set; }
}