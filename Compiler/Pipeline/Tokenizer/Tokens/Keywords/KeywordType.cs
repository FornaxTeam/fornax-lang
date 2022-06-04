using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Keywords;

public enum KeywordType
{
    Import,
    Export,

    Struct,
    Interface,
    Implement,
    Enum,

    Const,
    Final,
    Var,

    Return,
    Break,
    Continue,

    If,
    While,
    For,
    Match,

    As,
    Is,
}
