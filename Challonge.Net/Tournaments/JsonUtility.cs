using System;
using System.Collections.Generic;
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
                "in_progress" => TournamentState.Underway,
                "ended" => TournamentState.Completed,
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
                case TournamentState.Underway:
                    writer.WriteStringValue("in_progress");
                    break;
                case TournamentState.Completed:
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

    internal class TournamentRankingStatsJsonConverter : JsonConverter<TournamentRankingStats>
    {
        public override TournamentRankingStats Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumValue = reader.GetString();

            return enumValue switch
            {
                "match wins" => TournamentRankingStats.MatchWins,
                "game wins" => TournamentRankingStats.GameWins,
                "game win percentage" => TournamentRankingStats.GameWinPercentage,
                "points scored" => TournamentRankingStats.PointsScored,
                "points difference" => TournamentRankingStats.PointsDifference,
                "custom" => TournamentRankingStats.Custom,
                _ => TournamentRankingStats.Invalid
            };
        }

        public override void Write(Utf8JsonWriter writer, TournamentRankingStats value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case TournamentRankingStats.MatchWins:
                    writer.WriteStringValue("match wins");
                    break;
                case TournamentRankingStats.GameWins:
                    writer.WriteStringValue("game wins");
                    break;
                case TournamentRankingStats.GameWinPercentage:
                    writer.WriteStringValue("game win percentage");
                    break;
                case TournamentRankingStats.PointsScored:
                    writer.WriteStringValue("points scored");
                    break;
                case TournamentRankingStats.PointsDifference:
                    writer.WriteStringValue("points difference");
                    break;
                case TournamentRankingStats.Custom:
                    writer.WriteStringValue("custom");
                    break;
            }
        }
    }

    internal class TournamentTieBreakArrayJsonConverter : JsonConverter<TournamentTieBreak[]>
    {
        public override TournamentTieBreak[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<TournamentTieBreak> result = new List<TournamentTieBreak>();

            reader.Read();

            while (reader.TokenType != JsonTokenType.EndArray)
            {
                string value = reader.GetString();

                TournamentTieBreak parsed = value switch
                {
                    "match wins" => TournamentTieBreak.MatchWins,
                    "game wins" => TournamentTieBreak.GameWins,
                    "game win percentage" => TournamentTieBreak.GameWinPercentage,
                    "points scored" => TournamentTieBreak.PointsScored,
                    "points difference" => TournamentTieBreak.PointsDifference,
                    "match wins vs tied" => TournamentTieBreak.WinsVSTiedParticipants,
                    "median buchholz" => TournamentTieBreak.MedianBuchholz,
                    _ => TournamentTieBreak.Invalid
                };

                result.Add(parsed);
                reader.Read();
            }

            return result.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, TournamentTieBreak[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray("tie_breaks");

            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case TournamentTieBreak.MatchWins:
                        writer.WriteStringValue("match wins");
                        break;
                    case TournamentTieBreak.GameWins:
                        writer.WriteStringValue("game wins");
                        break;
                    case TournamentTieBreak.GameWinPercentage:
                        writer.WriteStringValue("game win percentage");
                        break;
                    case TournamentTieBreak.PointsScored:
                        writer.WriteStringValue("points scored");
                        break;
                    case TournamentTieBreak.PointsDifference:
                        writer.WriteStringValue("points difference");
                        break;
                    case TournamentTieBreak.WinsVSTiedParticipants:
                        writer.WriteStringValue("match wins vs tied");
                        break;
                    case TournamentTieBreak.MedianBuchholz:
                        writer.WriteStringValue("median buchholz");
                        break;
                }
            }

            writer.WriteEndArray();
        }
    }
}
