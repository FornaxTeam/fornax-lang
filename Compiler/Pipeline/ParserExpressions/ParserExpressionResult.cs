namespace Fornax.Compiler.Pipeline.ParserExpressions;

public record ParserExpressionResult(string? ErrorMessage = null)
{
    public bool IsSuccessful => ErrorMessage is null;
}
