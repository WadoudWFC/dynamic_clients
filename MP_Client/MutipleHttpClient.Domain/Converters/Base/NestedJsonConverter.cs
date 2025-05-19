using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Converters.Base;

public class NestedJsonConverter : JsonConverter<object>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            if (root.ValueKind == JsonValueKind.String)
            {
                return JsonSerializer.Deserialize(root.GetString()!, typeToConvert, options)!;
            }
            return JsonSerializer.Deserialize(root.GetRawText(), typeToConvert, options)!;
        }
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
