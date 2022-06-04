using Fornax.Compiler.Logging;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler.ParserGenerator;

public delegate T Callable<T>(Pipe<Token> pipe, WriteLog log);
