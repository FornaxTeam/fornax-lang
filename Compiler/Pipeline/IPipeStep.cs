using Fornax.Compiler.Logging;

namespace Fornax.Compiler.Pipeline;

public interface IPipeStep<From, To>
{
    To? Execute(Pipe<From> pipe, WriteLog log);
}
