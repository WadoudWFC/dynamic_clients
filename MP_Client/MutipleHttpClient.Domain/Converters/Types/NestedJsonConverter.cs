using System.Text.Json;
using System.Text.Json.Serialization;
using MutipleHttpClient.Domain.Converters.Base;

namespace MutipleHttpClient.Domain;

public class NestedJsonConverter<T> : JsonConverter<T>
{
    private static readonly JsonConverter<object> _baseConverter = new NestedJsonConverter();
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (T)_baseConverter.Read(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        _baseConverter.Write(writer, value, options);
    }
}
