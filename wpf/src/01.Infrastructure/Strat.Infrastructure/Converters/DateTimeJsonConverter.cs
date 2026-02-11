using System.Text.Json;
using System.Text.Json.Serialization;

namespace Strat.Infrastructure.Converters
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private readonly string _format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (DateTime.TryParse(stringValue, out var date))
                {
                    return date;
                }
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }

    public class NullableDateTimeJsonConverter : JsonConverter<DateTime?>
    {
        private readonly string _format = "yyyy-MM-dd HH:mm:ss";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                {
                    return null;
                }
                if (DateTime.TryParse(stringValue, out var date))
                {
                    return date;
                }
            }
            else if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(_format));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}