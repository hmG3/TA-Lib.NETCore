using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TALib.NETCore.Tests.Models;

public class TestDataModel<T> where T : IFloatingPointIeee754<T>
{
    public required string Name { get; init; }

    public required T[][] Inputs { get; set; }

    public T[] Options { get; init; } = [];

    public int? Unstable { get; init; }

    public required T[][] Outputs { get; init; }

    [JsonConverter(typeof(RangeConverter))]
    public Range Range { get; init; }

    public bool Skip { get; init; }

    public override string ToString() => Name;
}

internal sealed class RangeConverter : JsonConverter<Range>
{
    public override Range Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();
        var start = reader.GetInt32();
        reader.Read();
        var end = reader.GetInt32();
        reader.Read();

        return new Range(start, end);
    }

    public override void Write(Utf8JsonWriter writer, Range value, JsonSerializerOptions options) => throw new NotSupportedException();
}
