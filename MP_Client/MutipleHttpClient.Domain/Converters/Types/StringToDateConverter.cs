using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Converters.Types;

public class StringToDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String) return DateTime.ParseExact(reader.GetString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd/MM/yyyy HH:mm:ss"));
    }
}
