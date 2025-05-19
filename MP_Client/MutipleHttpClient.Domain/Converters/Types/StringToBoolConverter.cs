using System.Text.Json;
using System.Text.Json.Serialization;


namespace MutipleHttpClient.Domain.Converters.Types;

public class StringToBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String) return bool.Parse(reader.GetString());
        return reader.GetBoolean();
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "True" : "False");
    }
}
