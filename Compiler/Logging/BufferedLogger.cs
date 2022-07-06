using System.Collections.Generic;
using System.Linq;

namespace Fornax.Compiler.Logging;

public class BufferedLogger
{
    private readonly Queue<(string message, ErrorLevel errorLevel, long start, long end)> queue = new();

    public bool HasErrors => CriticalCount != 0;

    public bool HasWarnings => WarningCount != 0;

    public int CriticalCount { get; private set; } = 0;

    public int WarningCount { get; private set; } = 0;

    public int WarningLength => queue.Sum(entry => entry.errorLevel == ErrorLevel.Warning ? (int)(entry.end - entry.start) : 0);

    public int CriticalLength => queue.Sum(entry => entry.errorLevel == ErrorLevel.Critical ? (int)(entry.end - entry.start) : 0);

    public WriteLog Log => (message, errorLevel, start, end) =>
    {
        queue.Enqueue((message, errorLevel, start, end));

        if (errorLevel == ErrorLevel.Warning)
        {
            WarningCount++;
        }
        else if (errorLevel == ErrorLevel.Critical)
        {
            CriticalCount++;
        }
    };

    public bool WriteTo(WriteLog writeLog)
    {
        var successful = true;

        while (queue.Count > 0)
        {
            var (message, errorLevel, start, end) = queue.Dequeue();

            if (errorLevel == ErrorLevel.Critical)
            {
                successful = false;
            }

            writeLog(message, errorLevel, start, end);
        }

        WarningCount = CriticalCount = 0;

        return successful;
    }
}