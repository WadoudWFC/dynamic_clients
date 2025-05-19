using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Converters.Base;

public class NestedJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // We can convert any type
        return true;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var convertType = typeof(NestedJsonConverter).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(convertType)!;
    }
}
