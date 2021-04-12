using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Challonge.Matches;

namespace Challonge.Json
{
    internal class MatchStateJsonConverter : JsonConverter<MatchState>
    {
        public override MatchState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumValue = reader.GetString();

            switch (enumValue)
            {
                case "pending":
                    return MatchState.Pending;
                case "open":
                    return MatchState.Open;
                case "complete":
                    return MatchState.Complete;
                default:
                    return MatchState.Invalid;
            }
        }

        public override void Write(Utf8JsonWriter writer, MatchState value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case MatchState.Pending:
                    writer.WriteStringValue("pending");
                    break;
                case MatchState.Open:
                    writer.WriteStringValue("open");
                    break;
                case MatchState.Complete:
                    writer.WriteStringValue("complete");
                    break;
            }
        }
    }
}
