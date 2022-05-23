using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fornax.Compiler;

public static class ColoredConsole
{
    public static void WriteLine(string message)
    {
        Write(message);
        Console.WriteLine();
    }

    public static void Write(string message)
    {
        if (message is null)
        {
            return;
        }

        Stack<ConsoleColor> stack = new();
        stack.Push(Console.ForegroundColor = ConsoleColor.Gray);

        lock (Console.Out)
        {
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
        }
    }
}
