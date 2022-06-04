using Fornax.Compiler.ParserGenerator;
using System.Collections.Generic;

namespace Fornax.Compiler.Logging;

public class BufferedLogger
{
    private readonly Queue<(string message, ErrorLevel errorLevel, long start, long end)> queue = new();

    public WriteLog WriteLog => (message, errorLevel, start, end) => queue.Enqueue((message, errorLevel, start, end));

    public void WriteTo(WriteLog writeLog)
    {
        while (queue.Count > 0)
        {
            var (message, errorLevel, start, end) = queue.Dequeue();
            writeLog(message, errorLevel, start, end);
        }
    }
}
