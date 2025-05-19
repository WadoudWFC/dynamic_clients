using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MutipleHttpClient.Domain.Converters.Types
{
    public class JsonToFormattedDateConverter : JsonConverter<DateTime?>
    {
        private static readonly string[] SupportedFormats =
        {
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-ddTHH:mm:ss.ff",
            "MM/dd/yyyy HH:mm:ss",
            "yyyy-MM-dd"
        };
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            if (string.IsNullOrEmpty(dateString)) return null;
            // Try all supported formats
            foreach (var format in SupportedFormats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return date;
            }

            // Fallback to standard DateTime parsing
            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fallbackDate))
                return fallbackDate;
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
