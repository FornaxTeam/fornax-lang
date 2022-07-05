using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasm.CLI.Commands;

public class HelpCommand : Command
{
    public override string[] Arguments => Array.Empty<string>();

    public override string Description => "Shows a list of all commands.";

    public override string Name => "help";

    public override void Run(CommandLineInterface cli, Dictionary<string, string> arguments)
    {
        var spaces = cli.Commands
            .Select(command => command.Usage.Length)
            .Max();

        foreach (var command in cli.Commands)
        {
            CommandLineInterface.Log($"§e{command.Usage}§r{new string(' ', spaces - command.Usage.Length)}: {command.Description}");
        }
    }
}
