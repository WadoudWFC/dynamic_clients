using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain;

public class FlexibleDateTimeConverter : JsonConverter<DateTime?>
{
    private readonly string[] _dateFormats = new[]
    {
        "yyyy-MM-ddTHH:mm:ss.fff",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd",
        "dd/MM/yyyy HH:mm:ss",
        "dd/MM/yyyy",
        "MM/dd/yyyy HH:mm:ss",
        "MM/dd/yyyy"
    };

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var dateString = reader.GetString();

            if (string.IsNullOrWhiteSpace(dateString) || dateString == "null")
            {
                return null;
            }

            foreach (var format in _dateFormats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    return date;
                }
            }

            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var generalDate))
            {
                return generalDate;
            }
            return null;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}