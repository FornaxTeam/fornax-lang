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

    public abstract long Position { get; set; }

    public abstract long Length { get; }

    public abstract T ReadNext();

    public abstract bool HasNext { get; }

    public Pipe<T2> Step<T2>(IPipeStep<T, T2> step) where T2 : class => new SteppedPipe<T, T2>(this, step);

    public List<T?> Finalize()
    {
        var oldPosition = Position;
        Position = 0;

        T? current;
        List<T?> result = new();

        while (true)
        {
            current = ReadNext();

            if (current is null)
            {
                result.Add(HasNext
                    ? throw new InvalidDataException("Unreaded data available.")
                    : default);

                break;
            }

            result.Add(current);
        }

        Position = oldPosition;

        return result;
    }

    public bool Fallback(Func<bool> action)
    {
        var fallback = Position;

        if (!action())
        {
            Position = fallback;
            return false;
        }

        return true;
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
