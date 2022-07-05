using System;
using System.Collections.Generic;
using System.Linq;
using Fornax.Compiler.Logging;
using Fornax.Compiler.Pipeline;
using Fornax.Compiler.Pipeline.Expressionizer;
using Fornax.Compiler.Pipeline.Tokenizer;

namespace Fornax.Compiler;

public static class MainClass
{
    public static void Main()
    {
#if DEBUG
        var currentDirectory = Environment.CurrentDirectory;
        var directory = @"\Compiler\";
        Environment.CurrentDirectory = currentDirectory[..(currentDirectory.IndexOf(directory) + directory.Length)];
#endif

        static void Log(string message, ErrorLevel errorLevel, long start, long end = -1)
        {
            var prefix = "§f[" + errorLevel switch
            {
                ErrorLevel.Critical => "§cCRIT",
                ErrorLevel.Warning => "§eWARN",
                ErrorLevel.Info => "§3INFO",
                _ => "§7NONE"
            } + "§r]: §7";

            ColoredConsole.WriteLine(prefix + message.Replace("\n", "\n" + prefix) + $" §8({start}, {end})");
        }

        var source = Source.FromFile("Script.fdx");

        Console.WriteLine(source + "\n");

        var tokens = source.Step(new TokenizerStep());

        var expressions = tokens
            .Step(new ExpressionizerStep());

        expressions
            .Finalize(Log)
            .Cast<IValueExpression>()
            .ForEach(Console.WriteLine);

        Console.ReadKey();
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var entry in source)
        {
            action(entry);
        }
    }

    public static IEnumerable<T> Handle<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var entry in source)
        {
            action(entry);
        }

        return source;
    }
}
