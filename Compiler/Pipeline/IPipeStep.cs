namespace Fornax.Compiler.Pipeline;

public interface IPipeStep<From, To>
    where From : struct
    where To : struct
{
    To? Execute(Pipe<From> pipe);
}
