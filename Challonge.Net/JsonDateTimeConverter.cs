using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Challonge.Json
{
    internal class JsonDateTimeConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            if (value == "null")
                return null;

            if (value.Contains('+'))
            {
                string[] time = value.Split('+');
                DateTimeOffset dateTime = DateTimeOffset.Parse(time[0]);
                string hours = time[1].Split(':')[0];
                dateTime.AddHours(int.Parse(hours));
                return dateTime;
            }
            else
            {
                //Console.WriteLine(value);
                string[] time = value.Split('-');
                DateTimeOffset dateTime = DateTimeOffset.Parse(time[0]);
                string hours = time[1].Split(':')[0];
                dateTime.AddHours(-int.Parse(hours));
                return dateTime;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
