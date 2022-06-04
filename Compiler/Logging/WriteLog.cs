using Fornax.Compiler.ParserGenerator;

namespace Fornax.Compiler.Logging;

public delegate void WriteLog(string message, ErrorLevel errorLevel, long start, long end = -1);
