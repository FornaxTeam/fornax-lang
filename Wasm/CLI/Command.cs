namespace Wasm.CLI;

public abstract class Command
{
    public abstract string[] Arguments { get; }

    public abstract string Description { get; }

    public abstract string Name { get; }

    public abstract void Run(CommandLineInterface cli, Dictionary<string, string> arguments);

    public string Usage => string.Join(' ', new[] { Name }.Concat(Arguments.Select(arg => $"<{arg}>")));
}