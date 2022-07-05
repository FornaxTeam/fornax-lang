using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.CLI.Commands;

public class StatusCommand : Command
{
    public override string[] Arguments => Array.Empty<string>();

    public override string Description => "Shows the status of the current folder.";

    public override string Name => "status";

    public override void Run(CommandLineInterface cli, Dictionary<string, string> arguments)
    {
        DirectoryInfo directory = new(Environment.CurrentDirectory);

        CommandLineInterface.Log(directory.FullName);
    }
}
