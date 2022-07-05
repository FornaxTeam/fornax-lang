using System;
using Wasm.CLI;

const string NAME = "wasm";

var fallbackColor = Console.ForegroundColor;

CommandLineInterface cli = new();

static void PrintHelpMessage() => CommandLineInterface.Log($"§eUse '{NAME} help' for more informations.");

if (args.Length == 0)
{
    CommandLineInterface.Log("§cSubcommand expected.");
    PrintHelpMessage();
}
else
{
    cli.Run(args);
}

Console.ForegroundColor = fallbackColor;