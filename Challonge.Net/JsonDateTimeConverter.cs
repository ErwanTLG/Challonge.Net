using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Challonge.Json
{
    internal class JsonDateTimeConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            if (value == "null")
                return null;

            //return DateTimeOffset.ParseExact(value, "O", CultureInfo.InvariantCulture);
            return DateTimeOffset.ParseExact(value, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
