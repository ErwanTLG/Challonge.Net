using Challonge.Matches.Json;
using System;
using System.Text.Json.Serialization;

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
        public int? AttachmentCount { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }

        [JsonPropertyName("has_attachment")]
        public bool HasAttachment { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("loser_id")]
        public int? LoserId { get; set; }

        [JsonPropertyName("player1_id")]
        public int? Player1Id { get; set; }

        [JsonPropertyName("player1_is_prereq_match_loser")]
        public bool Player1IsPrereqMatchLoser { get; set; }

        [JsonPropertyName("player1_prereq_match_id")]
        public int? Player1PrereqMatchId { get; set; }

        [JsonPropertyName("player1_votes")]
        public int? Player1Votes { get; set; }

        [JsonPropertyName("player2_id")]
        public int? Player2Id { get; set; }

        [JsonPropertyName("player2_is_prereq_match_loser")]
        public bool Player2IsPrereqMatchLoser { get; set; }

        [JsonPropertyName("player2_prereq_match_id")]
        public int? Player2PrereqMatchId { get; set; }

        [JsonPropertyName("player2_votes")]
        public int? Player2Votes { get; set; }

        [JsonPropertyName("round")]
        public int Round { get; set; }

        [JsonPropertyName("scheduled_time")]
        public DateTimeOffset? ScheduledTime { get; set; }

        [JsonPropertyName("started_at")]
        public DateTimeOffset? StartedAt { get; set; }

        [JsonPropertyName("state")]
        [JsonConverter(typeof(MatchStateJsonConverter))]
        public MatchState State { get; set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        [JsonPropertyName("underway_at")]
        public DateTimeOffset? UnderwayAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonPropertyName("winner_id")]
        public int? WinnerId { get; set; }

        [JsonPropertyName("prerequisite_match_ids_csv")]
        public string PrerequisiteMatchIdsCsv { get; set; }

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
