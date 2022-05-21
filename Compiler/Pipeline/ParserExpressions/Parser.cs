using System;

namespace Fornax.Compiler.Pipeline.ParserExpressions;

public class Parser<TParserType>
{
    private readonly Action<long, long, ErrorLevel, string> writeLogEntry;

    private Parser(Action<long, long, ErrorLevel, string> writeLogEntry) => this.writeLogEntry = writeLogEntry;

    public ParserExpression<T, TParserType> Expect<T>(params Func<T, bool>[] conditions) where T : TParserType => new
    (
        this,
        conditions,
        Array.Empty<Action<T>>(),
        false
    );

    public void WriteLogEntry(long start, long end, ErrorLevel errorLevel, string message) => writeLogEntry(start, end, errorLevel, message);

    public static Parser<TParserType> Create(Action<long, long, ErrorLevel, string> writeLogEntry) => new(writeLogEntry);
}
