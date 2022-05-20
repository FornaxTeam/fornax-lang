using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline;

public abstract class Pipe<T>
    where T : struct
{
    private class SteppedPipe<From, To> : Pipe<To>
        where From : struct
        where To : struct
    {
        private readonly Pipe<From> pipe;
        private readonly IPipeStep<From, To> step;

        public SteppedPipe(Pipe<From> pipe, IPipeStep<From, To> step)
        {
            this.pipe = pipe;
            this.step = step;
        }

        public override long Position
        {
            get => pipe.Position;
            set => pipe.Position = value;
        }

        public override To? ReadNext() => step.Execute(pipe);
    }

    public abstract long Position { get; set; }

    public abstract T? ReadNext();

    public Pipe<T2> Step<T2>(IPipeStep<T, T2> step) where T2 : struct => new SteppedPipe<T, T2>(this, step);

    public IEnumerable<T?> Finalize()
    {
        T? current;

        while (true)
        {
            current = ReadNext();

            if (current is null)
            {
                yield return null;
                break;
            }

            yield return current;
        }
    }
}
