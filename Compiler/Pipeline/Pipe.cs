using Fornax.Compiler.Logging;
using System;
using System.Collections.Generic;
using System.IO;

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

        public override To ReadNext(WriteLog log) => step.Execute(pipe, log)!;

        public override bool HasNext => pipe.HasNext;

        public override long Length => pipe.Length;
    }

    public abstract long Length { get; }

    public abstract long Position { get; set; }

    public abstract T ReadNext(WriteLog log);

    public abstract bool HasNext { get; }

    public Pipe<T2> Step<T2>(IPipeStep<T, T2> step) where T2 : class => new SteppedPipe<T, T2>(this, step);

    public IEnumerable<T?> Finalize(WriteLog log)
    {
        var oldPosition = Position;
        Position = 0;

        T? current;

        while (HasNext)
        {
            current = ReadNext(log);

            if (current is null)
            {
                yield return HasNext
                    ? throw new InvalidDataException("Unreaded data available.")
                    : default;

                break;
            }

            yield return current;
        }

        Position = oldPosition;
    }

    public bool Fallback(Func<long, bool> action)
    {
        var fallback = Position;

        if (!action(fallback))
        {
            Position = fallback;
            return false;
        }

        return true;
    }
}
