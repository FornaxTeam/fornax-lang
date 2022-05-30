using System;

namespace Fornax.Compiler.ParserGenerator.Interfaces;

public interface ICallResult<T>
{
    ICallResult<T> Handle(Action<T> handler);

    void Execute(T obj);

    IParserFragment Ok();
}
