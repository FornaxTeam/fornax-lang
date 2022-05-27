using Fornax.Compiler.Pipeline.Tokenizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

        public override long Length => pipe.Length;
    }

    public abstract long Length { get; }

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
                yield return HasNext
                    ? throw new InvalidDataException("Unreaded data available.")
                    : default;

                break;
            }

            yield return current;
        }

        Position = 0;
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

    public T2? Expect<T2>() where T2 : T
    {
        T2? result = default;

        Fallback(fallbackPosition =>
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
