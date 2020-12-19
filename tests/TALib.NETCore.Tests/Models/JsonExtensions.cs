using System;
using System.Buffers;
using System.Text.Json;

namespace TALib.NETCore.Tests.Models
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions? options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions? options = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

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

            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
        }

        public static object ToObject(this JsonDocument document, Type returnType, JsonSerializerOptions? options = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return document.RootElement.ToObject(returnType, options);
        }
    }
}
