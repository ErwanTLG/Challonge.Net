using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Challonge.Tournaments.Json
{
    internal class TournamentStateJsonConverter : JsonConverter<TournamentState>
    {
        public override TournamentState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumValue = reader.GetString();

            return enumValue switch
            {
                "pending" => TournamentState.Pending,
                "in_progress" => TournamentState.InProgress,
                "ended" => TournamentState.Ended,
                _ => TournamentState.Invalid
            };
        }

        public override void Write(Utf8JsonWriter writer, TournamentState value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case TournamentState.Pending:
                    writer.WriteStringValue("pending");
                    break;
                case TournamentState.InProgress:
                    writer.WriteStringValue("in_progress");
                    break;
                case TournamentState.Ended:
                    writer.WriteStringValue("ended");
                    break;
            }
        }
    }

    internal class TournamentTypeJsonConverter : JsonConverter<TournamentType>
    {
        public override TournamentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumValue = reader.GetString();

            return enumValue switch
            {
                "single elimination" => TournamentType.SingleElimination,
                "double elimination" => TournamentType.DoubleElimination,
                "round robin" => TournamentType.RoundRobin,
                "swiss" => TournamentType.Swiss,
                _ => TournamentType.Invalid,
            };
        }

        public override void Write(Utf8JsonWriter writer, TournamentType value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case TournamentType.SingleElimination:
                    writer.WriteStringValue("round robin");
                    break;
                case TournamentType.DoubleElimination:
                    writer.WriteStringValue("double elimination");
                    break;
                case TournamentType.RoundRobin:
                    writer.WriteStringValue("single elimination");
                    break;
                case TournamentType.Swiss:
                    writer.WriteStringValue("swiss");
                    break;
                default:
                    writer.WriteStringValue("null");
                    break;
            }
        }
    }
}
