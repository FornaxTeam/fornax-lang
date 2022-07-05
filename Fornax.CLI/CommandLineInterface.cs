using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fornax.CLI;

public class CommandLineInterface
{
    public Command[] Commands { get; private set; } = null!;

    public CommandLineInterface() => LoadCommands(Assembly.GetAssembly(typeof(Command))!.GetTypes());

    private static void CommandNotFound(string name) => Log($"§cThe command '{name}' was not found.");

    private static void MissingArgument(string name, string usage) => Log($"§cThe argument '{name}' is missing.\nUsage: §e{usage}");

    private static void ToManyArguments(string usage) => Log($"§cTo many arguments.\nUsage: §e{usage}");

    public static void Log(object? obj = null)
    {
        Write(obj);
        Console.WriteLine();
    }

    private static void Write(object? obj, int left = -1, int top = -1)
    {
        var posX = left == -1 ? Console.CursorLeft : left;
        var posY = top == -1 ? Console.CursorTop : top;

        var message = "§7" + (obj?.ToString() ?? "");

        Stack<ConsoleColor> stack = new();
        stack.Push(Console.ForegroundColor = ConsoleColor.Gray);

        lock (Console.Out)
        {
            var (oldX, oldY) = Console.GetCursorPosition();

            Console.CursorLeft = posX;
            Console.CursorTop = posY;

            var startColor = Console.ForegroundColor;
            var match = Regex.Match(message, "(?<text>(.|\n)*?)§(?<color>[0-9A-Fa-fRr])");

            while (match.Success)
            {
                message = message[match.Length..];

                Console.Write(match.Groups["text"].Value);

                var color = match.Groups["color"].Value;

                if (color.Equals("R", StringComparison.OrdinalIgnoreCase) && stack.Count != 0)
                {
                    stack.Pop();
                }
                else
                {
                    stack.Push((ConsoleColor)Convert.ToByte(color, 16));
                }

                Console.ForegroundColor = stack.Peek();

                match = Regex.Match(message, "(?<text>(.|\n)*?)§(?<color>[0-9A-Fa-fRr])");
            }

            Console.Write(message);

            if (left != -1)
            {
                Console.CursorLeft = oldX;
                Console.CursorTop = oldY;
            }
        }
    }

    public void LoadCommands(Type[] types) => Commands = types
        .Where(type => type.IsAssignableTo(typeof(Command)) && type != typeof(Command))
        .Select(type => type.GetConstructors()[0].Invoke(Array.Empty<object>()))
        .Cast<Command>()
        .ToArray();

    public void Run(string command) => Run(command.Split(' ', StringSplitOptions.RemoveEmptyEntries));

    public void Run(string[] command) => Run(command[0], command[1..]);

    private void Run(string commandName, string[] arguments)
    {
        foreach (var command in Commands)
        {
            if (command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
            {
                Run(command, arguments);
                return;
            }
        }

        CommandNotFound(commandName);
    }

    private void Run(Command command, string[] arguments)
    {
        Dictionary<string, string> argumentMap = new();

        if (arguments.Length < command.Arguments.Length)
        {
            MissingArgument(command.Usage, command.Arguments[arguments.Length]);
            return;
        }
        else if (arguments.Length > command.Arguments.Length)
        {
            ToManyArguments(command.Usage);
            return;
        }

        command.Run(this, argumentMap);
    }
}
