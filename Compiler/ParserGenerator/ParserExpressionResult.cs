namespace Fornax.Compiler.ParserGenerator;

public record ParserExpressionResult(params string[] ErrorMessages)
{
    public bool IsSuccessful => ErrorMessages.Length == 0;
}
