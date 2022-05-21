using System;
using System.Collections.Generic;

namespace Fornax.Compiler.Pipeline;

public abstract class Pipe<T>
{
    private class SteppedPipe<From, To> : Pipe<To>
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

        public override To ReadNext() => step.Execute(pipe)!;

        public override bool HasNext => pipe.HasNext;
    }

    public abstract long Position { get; set; }

    public abstract T ReadNext();

    public abstract bool HasNext { get; }

    public Pipe<T2> Step<T2>(IPipeStep<T, T2> step) where T2 : class => new SteppedPipe<T, T2>(this, step);

    public IEnumerable<T?> Finalize()
    {
        T? current;

        while (true)
        {
            current = ReadNext();

            if (current is null)
            {
                yield return default;
                break;
            }

            yield return current;
        }
    }

    public void Fallback(Func<bool> action)
    {
        var fallback = Position;

        if (!action())
        {
            Position = fallback;
        }
    }

    public T2? Expect<T2>() where T2 : T
    {
        T2? result = default;

        Fallback(() =>
        {
            if (ReadNext() is T2 element)
            {
                result = element;
                return true;
            }

            return false;
        });

        return result;
    }
}
