namespace Fornax.Compiler.Pipeline.Tokenizer.Tokens.Comparison;

public enum Comparison
{
    Equal,
    NotEqual,
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual
}

public static class ComparisonExtension
{
    public static bool NeedsNumbers (this Comparison c)
    {
        switch (c) {
        case Comparison.Equal:
        case Comparison.NotEqual:
        case Comparison.Greater:
        case Comparison.GreaterOrEqual:
        case Comparison.Less:
        case Comparison.LessOrEqual:
            return true;
        default:
            return false;
        }
    }
}