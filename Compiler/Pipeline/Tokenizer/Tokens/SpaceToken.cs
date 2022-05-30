using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens;

public class SpaceToken : Token
{
    protected override bool Read(Pipe<char?> pipe)
    {
        while (pipe.HasNext)
        {
            var @char = pipe.ReadNext()!.Value;

            if (!char.IsWhiteSpace(@char))
            {
                pipe.Position--;
                return true;
            }
        }

        return true;
    }
}

