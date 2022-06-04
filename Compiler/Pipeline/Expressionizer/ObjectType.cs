namespace Fornax.Compiler.Pipeline.Expressionizer;

public record ObjectType(string Name, ObjectType[] GenericArguments, int[] ArrayDimensions);
