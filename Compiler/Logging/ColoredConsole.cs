using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fornax.Compiler.Logging;

public static class ColoredConsole
{
    public static void WriteLine(object? obj = null)
    {
        Write(obj);
        Console.WriteLine();
    }

    public static void Write(object? obj, int left = -1, int top = -1)
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
}
