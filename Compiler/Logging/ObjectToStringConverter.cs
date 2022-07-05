using System;
using System.Linq;
using System.Text;

namespace Fornax.Compiler.Logging;

public static class ObjectToStringConverter
{
    public static string ToString(object? obj)
    {
        const string space = "   ";

        if (obj is null)
        {
            return "NULL";
        }

        var type = obj.GetType();

        if (type.IsPrimitive)
        {
            return obj.ToString() ?? "NULL";
        }

        if (obj is string str)
        {
            return $"\"{str.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
        }

        static string ArrayToString(Array array)
        {
            StringBuilder result = new();
            result.Append("[\n");

            foreach (var item in array)
            {
                result.Append($"{space}{ToString(item)?.Replace("\n", $"\n{space}")}\n");
            }

            result.Append(']');
            return result.ToString();
        }

        var value = string.Join("\n", type.GetProperties()
            .Where(property => property.GetIndexParameters().Length == 0)
            .Select(property =>
            {
                var value = property.GetValue(obj);

                return value is Array array
                    ? $"{space}{property.Name}: {ArrayToString(array).Replace("\n", $"\n{space}")}"
                    : $"{space}{property.Name}: {ToString(value).Replace("\n", $"\n{space}")}";
            }));

        return $"{type.Name} {{\n{value}\n}}";
    }
}
