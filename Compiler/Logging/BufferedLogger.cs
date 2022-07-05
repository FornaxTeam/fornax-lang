using System.Collections.Generic;

namespace Fornax.Compiler.Logging;

public class BufferedLogger
{
    private readonly Queue<(string message, ErrorLevel errorLevel, long start, long end)> queue = new();

    private int warnings = 0;
    private int errors = 0;

    public bool HasErrors => errors != 0;

    public bool HasWarnings => warnings != 0;

    public WriteLog Log => (message, errorLevel, start, end) =>
    {
        queue.Enqueue((message, errorLevel, start, end));

        if (errorLevel == ErrorLevel.Warning)
        {
            warnings++;
        }
        else if (errorLevel == ErrorLevel.Critical)
        {
            errors++;
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

        warnings = errors = 0;

        return successful;
    }
}
