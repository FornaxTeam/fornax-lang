using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wasm.Binary;

public static class MainClass
{
    public static void Main()
    {
        Environment.CurrentDirectory = @"J:\IT-TEAM\Alex\TAS.AD\Test";

        const bool read = false;

        if (read)
        {
            using FileStream stream = new("Module.wasm", FileMode.Open, FileAccess.Read);
            var fileHeader = stream.Read(new ModuleHeader());
        }
        else
        {
            using FileStream stream = new("Module.wasm", FileMode.Create, FileAccess.Write);
            var versions = new StaticTypedVector<Version>
            (
                new(1, 2, 3, 4),
                new(5, 6, 7, 8),
                new(10, 11, 12, 13)
            );

            Console.WriteLine(versions.Length);
            stream.Write(versions);

            Console.ReadKey();
        }
    }
}

public class Module : BinaryTuple
{
    public ModuleHeader Header { get; set; } = new();

    public Module()
    {
        References = new Reference<IBinaryValue>[]
        {
            new(() => Header, value => Header = (ModuleHeader)value),
        };
    }
}

public enum SectionType : byte
{
    CustomSection = 0,
    TypeSection = 1,
    ImportSection = 2,
    FunctionSection = 3,
    TableSection = 4,
    MemorySection = 5,
    GlobalSection = 6,
    ExportSection = 7,
    StartSection = 8,
    ElementSection = 9,
    CodeSection = 10,
    DataSection = 11,
    DataCountSection = 12
}

public abstract class Section<T> : BinaryTuple where T : IBinaryValue
{
    private readonly BufferContainer<T> container;

    public abstract SectionType Type { get; }

    public T Body
    {
        get => container.Value;
        set => container.Value = value;
    }

    public override long Length => container.Length + 1;

    public Section(T body) => container = new BufferContainer<T>(body);

    public override bool ReadFrom(Stream stream) => stream.ReadByte() == (byte)Type && stream.Read(new BufferContainer<T>(default!)) is not null;

    public override void WriteTo(Stream stream)
    {
        stream.WriteByte((byte)Type);
        stream.Write(container);
    }
}

public static class BinaryExtensions
{
    public static IBinaryValue? Read(this Stream stream, IBinaryValue? value)
    {
        if (value is null)
        {
            return null;
        }

        var fallbackPosition = stream.Position;

        if (value.ReadFrom(stream))
        {
            return value;
        }

        stream.Position = fallbackPosition;
        return null;
    }

    public static T? Read<T>(this Stream stream, T value) => (T?)stream.Read((IBinaryValue?)value);

    public static void Write(this Stream stream, params IBinaryValue[] types)
    {
        foreach (var value in types)
        {
            stream.Write(value);
        }
    }

    public static void Write(this Stream stream, IBinaryValue value) => value.WriteTo(stream);
}

public interface IBinaryValue
{
    long Length { get; }

    bool ReadFrom(Stream stream);

    void WriteTo(Stream stream);
}

public class Optional<T> : IBinaryValue where T : IBinaryValue
{
    public T? Value { get; set; }

    public long Length => Value?.Length ?? 0;

    public bool ReadFrom(Stream stream)
    {
        Value = stream.Read(Value);
        return true;
    }

    public void WriteTo(Stream stream)
    {
        if (Value is not null)
        {
            stream.Write(Value);
        }
    }
}

public sealed class Reference<T>
{
    private readonly Func<T> getter;
    private readonly Action<T> setter;

    public Reference(Func<T> getter, Action<T> setter)
    {
        this.getter = getter;
        this.setter = setter;
    }

    public T Value
    {
        get => getter();
        set => setter(value);
    }
}

public sealed class UnsignedLeb128Number : IBinaryValue
{
    public long Value { get; private set; }

    public long Length
    {
        get
        {
            var length = 0;
            var value = Value;

            do
            {
                value >>= 7;
                length++;
            }
            while (value != 0);

            return length;
        }
    }

    public UnsignedLeb128Number()
    {
    }

    public UnsignedLeb128Number(long value) => Value = value;

    public bool ReadFrom(Stream stream)
    {
        var result = 0;
        var shift = 0;

        while (true)
        {
            var @byte = stream.ReadByte();
            result |= (@byte & 0x7f) << shift;

            if ((@byte & 0x80) == 0)
            {
                break;
            }

            shift += 7;
        }

        Value = result;
        return true;
    }

    public void WriteTo(Stream stream)
    {
        var value = Value;

        do
        {
            var @byte = value & 0x7f;
            value >>= 7;

            if (value != 0)
            {
                @byte |= 0x80;
            }

            stream.WriteByte((byte)@byte);
        }
        while (value != 0);
    }
}

public sealed class BufferContainer<T> : IBinaryValue where T : IBinaryValue
{
    public T Value { get; set; }

    public BufferContainer(T value) => Value = value;

    public long Length => new UnsignedLeb128Number(Value.Length).Length + Value.Length;

    public bool ReadFrom(Stream stream)
    {
        if (stream.Read(new UnsignedLeb128Number()) is null)
        {
            return false;
        }

        var value = stream.Read(Value);

        if (value is null)
        {
            return false;
        }

        Value = value;

        return true;
    }

    public void WriteTo(Stream stream)
    {
        stream.Write(new UnsignedLeb128Number(Value.Length));
        stream.Write(Value);
    }
}

public sealed class StaticTypedVector<T> : IBinaryValue where T : IBinaryValue, new()
{
    public List<T> Values { get; private set; } = new();

    public StaticTypedVector(params T[] values) => Values = values.ToList();

    public long Length => new UnsignedLeb128Number(Values.Count).Length + Values.Sum(value => value.Length);

    public bool ReadFrom(Stream stream)
    {
        List<T> values = new();
        var uLeb128 = stream.Read(new UnsignedLeb128Number());

        if (uLeb128 is null)
        {
            return false;
        }

        var length = uLeb128.Value;

        for (var i = 0; i < length; i++)
        {
            var value = stream.Read(new T());

            if (value is null)
            {
                return false;
            }

            values.Add(value);
        }

        Values = values;
        return true;
    }

    public void WriteTo(Stream stream)
    {
        stream.Write(new UnsignedLeb128Number(Values.Count));

        foreach (var value in Values)
        {
            stream.Write(value);
        }
    }
}

public class BinaryTuple : IBinaryValue
{
    public Reference<IBinaryValue>[] References { get; protected set; } = Array.Empty<Reference<IBinaryValue>>();

    public virtual long Length => References.Sum(reference => reference.Value.Length);

    public virtual bool ReadFrom(Stream stream)
    {
        foreach (var reference in References)
        {
            var resultType = stream.Read(reference.Value);

            if (resultType is null)
            {
                return false;
            }

            reference.Value = resultType;
        }

        return true;
    }

    public virtual void WriteTo(Stream stream)
    {
        foreach (var reference in References)
        {
            stream.Write(reference.Value);
        }
    }
}

public sealed class ModuleHeader : BinaryTuple
{
    public Version Version { get; set; } = new();

    public ModuleHeader()
    {
        References = new Reference<IBinaryValue>[]
        {
        new(() => new MagicHeader(), value => { }),
        new(() => Version, value => Version = (Version)value),
        };
    }
}

public sealed class Version : IBinaryValue
{
    public byte[] Data { get; } = new byte[] { 1, 0, 0, 0 };

    public Version()
    {
    }

    public Version(byte a, byte b, byte c, byte d)
    {
        Data[0] = a;
        Data[1] = b;
        Data[2] = c;
        Data[3] = d;
    }

    public long Length => 4;

    public bool ReadFrom(Stream stream) => stream.Read(Data) == 4;

    public void WriteTo(Stream stream) => stream.Write(Data);
}

public sealed class MagicHeader : IBinaryValue
{
    private readonly byte[] magicHeader = new byte[] { 0, 0x61, 0x73, 0x6d };

    public long Length => 4;

    public bool ReadFrom(Stream stream)
    {
        var data = new byte[4];
        stream.Read(data);
        return data.SequenceEqual(magicHeader);
    }

    public void WriteTo(Stream stream) => stream.Write(magicHeader);
}