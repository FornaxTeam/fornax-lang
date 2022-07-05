const string VERSION = "0.0.1";

static void PrintHelpMessage() => Console.WriteLine("Use 'fornax help' for more informations.");

static void InvalidArgumentsMessage()
{
    Console.WriteLine("Invalid arguments.");
    PrintHelpMessage();
}

if (args.Length == 0)
{
    Console.WriteLine("Subcommand expected.");
    PrintHelpMessage();

    return;
}

switch (args[0].ToLower())
{
    #region Basic Commands

    case "help":

        if (args.Length > 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine(
@"fornax <Subcommand> ...

Subcommands:
    Basic commands:
        help
            Shows this information text.

        version
            Shows the current version.

        new <name>
            Creates a new project.

        build
            Builds the current project.

        language-server
            Starts the fornax language server for the current project.

    Commands for wasm modules:
        bundle <input fornax module>... <output fornax module>
            Bundles two or more wasm modules to one module.
            The mappings become also bundled.

        read-meta <fornax module>
            Reads the fornax meta informations of a fornax module.

        write-meta <wasm module>
            Writes the fornax meta informations to a wasm module."
        );

        break;

    case "version":

        if (args.Length > 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("Version: " + VERSION);
        break;

    case "new":

        if (args.Length != 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("new");

        throw new NotImplementedException();

    case "build":

        if (args.Length != 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("build");

        throw new NotImplementedException();

    case "language-server":

        if (args.Length != 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("language-server");

        throw new NotImplementedException();

    #endregion

    #region Commands for wasm modules

    case "bundle":

        if (args.Length != 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("bundle");

        throw new NotImplementedException();

    case "read-meta":

        if (args.Length != 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("read-meta");

        throw new NotImplementedException();

    case "write-meta":

        if (args.Length != 1)
        {
            InvalidArgumentsMessage();
            return;
        }

        Console.WriteLine("write-meta");

        throw new NotImplementedException();

    #endregion

    default:

        Console.WriteLine("Invalid subcommand.");
        PrintHelpMessage();

        break;
}