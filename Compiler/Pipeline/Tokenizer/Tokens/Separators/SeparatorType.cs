using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Separators;

public enum SeparatorType
{
    Command, Value,
    Member, CollectionMember,

    ValueOpen, ValueClose,
    CollectionOpen, CollectionClose,
    BlockOpen, BlockClose,
}
