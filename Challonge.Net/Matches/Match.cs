using System;
using System.Text.Json.Serialization;
using Challonge.Matches.Json;

namespace Challonge.Matches
{
    internal class MatchData
    {
        [JsonPropertyName("match")]
        public Match Match { get; set; }
    }

    public class Match
    {
        [JsonPropertyName("attachment_count")]
        public int? AttachmentCount { get; internal set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; internal set; }

        [JsonPropertyName("group_id")]
        public int? GroupId { get; internal set; }

        [JsonPropertyName("has_attachment")]
        public bool HasAttachment { get; internal set; }

        [JsonPropertyName("id")]
        public int Id { get; internal set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; internal set; }

        [JsonPropertyName("location")]
        public string Location { get; internal set; }

        [JsonPropertyName("loser_id")]
        public int? LoserId { get; internal set; }

        [JsonPropertyName("player1_id")]
        public int? Player1Id { get; internal set; }

        [JsonPropertyName("player1_is_prereq_match_loser")]
        public bool Player1IsPrereqMatchLoser { get; internal set; }

        [JsonPropertyName("player1_prereq_match_id")]
        public int? Player1PrereqMatchId { get; internal set; }

        [JsonPropertyName("player1_votes")]
        public int? Player1Votes { get; set; }

        [JsonPropertyName("player2_id")]
        public int? Player2Id { get; internal set; }

        [JsonPropertyName("player2_is_prereq_match_loser")]
        public bool Player2IsPrereqMatchLoser { get; internal set; }

        [JsonPropertyName("player2_prereq_match_id")]
        public int? Player2PrereqMatchId { get; internal set; }

        [JsonPropertyName("player2_votes")]
        public int? Player2Votes { get; internal set; }

        [JsonPropertyName("round")]
        public int Round { get; internal set; }

        [JsonPropertyName("scheduled_time")]
        public DateTimeOffset? ScheduledTime { get; internal set; }

        [JsonPropertyName("started_at")]
        public DateTimeOffset? StartedAt { get; internal set; }

        [JsonPropertyName("state")]
        [JsonConverter(typeof(MatchStateJsonConverter))]
        public MatchState State { get; internal set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; internal set; }

        [JsonPropertyName("underway_at")]
        public DateTimeOffset? UnderwayAt { get; internal set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; internal set; }

        [JsonPropertyName("winner_id")]
        public int? WinnerId { get; set; }

        [JsonPropertyName("prerequisite_match_ids_csv")]
        public string PrerequisiteMatchIdsCsv { get; internal set; }

        [JsonPropertyName("scores_csv")]
        public string ScoresCsv { get; set; }
    }

    public enum MatchState
    {
        Invalid,
        Pending,
        Open,
        Complete
    }
}
