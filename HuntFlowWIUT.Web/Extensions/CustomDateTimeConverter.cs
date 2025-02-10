using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HuntFlowWIUT.Web.Extensions
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _format;
        public CustomDateTimeConverter(string format = "yyyy-MM-dd")
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();

            if (DateTime.TryParseExact(dateString, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }
            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
            {
                return dt;
            }
            throw new FormatException($"Unable to parse '{dateString}' as a valid DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}
