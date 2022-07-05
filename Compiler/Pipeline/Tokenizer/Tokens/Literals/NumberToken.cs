using Fornax.Compiler.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Literals;

public class NumberToken : Token
{
    protected override bool Read(Pipe<char?> pipe, WriteLog log)
    {
        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext(log)!.Value;

            if (!char.IsNumber(@char))
            {
                if (@char == '.')
                {
                    var oldPosition = pipe.Position;

                    while (pipe.HasNext)
                    {
                        @char = pipe.ReadNext(log)!.Value;

                        if (!char.IsNumber(@char))
                        {
                            pipe.Position--;
                            return true;
                        }
                    }

                    if (oldPosition != pipe.Position)
                        return true;
                }

                pipe.Position--;
                return true;
            }
        }

        return true;
    }
}
