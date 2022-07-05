using Fornax.Compiler.Logging;
using Fornax.Compiler.ParserGenerator;
using Fornax.Compiler.Pipeline.Expressionizer.Expressions.InnerBlock;
using Fornax.Compiler.Pipeline.Tokenizer;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens;
using Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Expressionizer.Expressions;

public record BlockExpression(long Start, long End, ICommandExpression[] Commands) : Expression(Start, End)
{
    public override string ToString() => base.ToString();

    public static BlockExpression Read(Pipe<Token> pipe, WriteLog log)
    {
        var start = pipe.Position;
        List<ICommandExpression> commands = new();

        ParserFragment.Create()
            .Expect<SeparatorToken>()
                .Where(token => token.Type == SeparatorType.BlockOpen)
                .MessageIfMissing("'{' expected.")
            .Call((Pipe<Token> pipe, WriteLog log) =>
            {
                var start = pipe.Position;

                var expressions = new[]
                {
                    CallExpression.ReadAsCommand
                };

                BufferedLogger? loggerOfResult = null;
                ICommandExpression? resultExpression = null;
                var endPosition = start;

                foreach (var read in expressions)
                {
                    pipe.Position = start;

                    BufferedLogger logger = new();

                    var expression = read(pipe, logger.Log);

                    if (resultExpression is null || !logger.HasErrors)
                    {
                        resultExpression = expression;
                        loggerOfResult = logger;
                        endPosition = pipe.Position;
                    }

                    if (!logger.HasErrors)
                    {
                        pipe.Position = endPosition;
                        break;
                    }
                }

                loggerOfResult?.WriteTo(log);

                return resultExpression!;
            })
                .Handle(commands.Add)
                .Ok()
            .Expect<SeparatorToken>()
                .Where(token => token.Type == SeparatorType.BlockClose)
                .MessageIfMissing("'}' expected.")
            .Parse(pipe, log);

        return new(start, pipe.Position, commands.ToArray());
    }
}
