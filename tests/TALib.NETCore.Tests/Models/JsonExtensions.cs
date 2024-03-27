using System;
using System.Buffers;
using System.Diagnostics;
using System.Text.Json;

namespace TALib.NETCore.Tests.Models;

public static class JsonExtensions
{
    public static T ToObject<T>(this JsonElement element, JsonSerializerOptions? options = null)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter))
        {
            element.WriteTo(writer);
        }

        var result = JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        Debug.Assert(result != null);
        return result;
    }

    public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(document);

        return document.RootElement.ToObject<T>(options);
    }

    public static object ToObject(this JsonElement element, Type returnType, JsonSerializerOptions? options = null)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter))
        {
            element.WriteTo(writer);
        }

        try
        {
            JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var result = JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
        Debug.Assert(result != null);
        return result;
    }

    public static object ToObject(this JsonDocument document, Type returnType, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(document);

        return document.RootElement.ToObject(returnType, options);
    }
}
