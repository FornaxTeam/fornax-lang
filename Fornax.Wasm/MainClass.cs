using Fornax.Wasm.BaseNodes;
using Fornax.Wasm.Modules;
using Fornax.Wasm.Sections.Type;
using System;
using System.IO;
using System.Linq;

namespace Fornax.Wasm;

public static class MainClass
{
    public static void Main()
    {
        Environment.CurrentDirectory = @"D:\WasmerTest\WasmInspector";

        Module? module = new()
        {
            TypeSection = new()
        };

        module.TypeSection.Types.Add(new FunctionType(new[] { BaseType.I32, BaseType.I32 }, new[] { BaseType.I32 }));

        // Write the module to the console.
        PrintNode(module);

        // Write the module to a file.
        using (var stream = new FileStream("Test.wasm", FileMode.Create))
        {
            module.Write(stream);
        }

        using NodeReader reader = new("Test.wasm");
        module = reader.Read<Module>();

        if (module is null)
        {
            Console.WriteLine("Error while reading.");
        }
        else
        {
            PrintNode(module);
        }

        Console.ReadKey();
    }

    public static void PrintNode(IBinaryNode node, int level = 0, bool isOptional = false)
    {
        var prefix = new string('|', level);

        if (node is ValueBasedBinaryNode valueNode)
        {
            using MemoryStream memoryStream = new();
            valueNode.Write(memoryStream);

            Console.WriteLine($"{prefix}+ {valueNode.GetType().Name}{(isOptional ? "?" : "")}");

            foreach (var chunk in memoryStream.ToArray().Chunk(16))
            {
                Console.WriteLine(prefix + "| " + BytesToString(chunk));
            }
        }
        else if (node is ChildBasedBinaryNode childNode)
        {
            Console.WriteLine($"{prefix}+ {childNode.GetType().Name}{(isOptional ? "?" : "")}");

            foreach (var child in childNode.Childs)
            {
                PrintNode(child, level + 1);
            }
        }
        else if (node is IHasBinaryNodeValue nodeWithValue)
        {
            if (nodeWithValue.Value is IBinaryNode value)
            {
                PrintNode(value, level + 1, true);
            }
            else
            {
                Console.WriteLine($"{prefix}+ {nodeWithValue.GetType().GetGenericArguments()[0].Name}?");
            }
        }
        else
        {
            throw new ArgumentException(null, nameof(node));
        }
    }

    public static string BytesToString(byte[] bytes) => string.Join(' ', bytes.Select(@byte => @byte.ToString("X2")));
}