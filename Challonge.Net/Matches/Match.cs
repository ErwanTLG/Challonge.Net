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
        /// <summary>
        /// Number of attachments of this match
        /// </summary>
        [JsonPropertyName("attachment_count")]
        public int? AttachmentCount { get; set; }

        /// <summary>
        /// When the match was created
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }

        /// <summary>
        /// Whether or not this match has any attachments
        /// </summary>
        [JsonPropertyName("has_attachment")]
        public bool HasAttachment { get; set; }

        /// <summary>
        /// The match's unique id
        /// </summary>
        /// <remarks>
        /// The match is attached to a tournament, so its id will be unique in the scope of that 
        /// tournament, but multiple matches may share the same id when they aren't part of the same tournament
        /// </remarks>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>
        /// The id of the participant that lost the match
        /// </summary>
        [JsonPropertyName("loser_id")]
        public int? LoserId { get; set; }

        /// <summary>
        /// The id of the first participant
        /// </summary>
        [JsonPropertyName("player1_id")]
        public int? Player1Id { get; set; }

        /// <summary>
        /// Whether or not the first player lost its previous match
        /// </summary>
        [JsonPropertyName("player1_is_prereq_match_loser")]
        public bool Player1IsPrereqMatchLoser { get; set; }

        /// <summary>
        /// The id of player 1's last match
        /// </summary>
        [JsonPropertyName("player1_prereq_match_id")]
        public int? Player1PrereqMatchId { get; set; }

        [JsonPropertyName("player1_votes")]
        public int? Player1Votes { get; set; }

        /// <summary>
        /// The id of the second participant
        /// </summary>
        [JsonPropertyName("player2_id")]
        public int? Player2Id { get; set; }

        /// <summary>
        /// Whether or not the second player lost its previous match
        /// </summary>
        [JsonPropertyName("player2_is_prereq_match_loser")]
        public bool Player2IsPrereqMatchLoser { get; set; }

        /// <summary>
        /// The id of player 2's last match
        /// </summary>
        [JsonPropertyName("player2_prereq_match_id")]
        public int? Player2PrereqMatchId { get; set; }

        [JsonPropertyName("player2_votes")]
        public int? Player2Votes { get; set; }

        /// <summary>
        /// The round of this match
        /// </summary>
        /// <remarks>
        /// Rounds start at index 1 and lower brackets matches have a negative round
        /// </remarks>
        [JsonPropertyName("round")]
        public int Round { get; set; }

        /// <summary>
        /// When this match is scheduled to start
        /// </summary>
        [JsonPropertyName("scheduled_time")]
        public DateTimeOffset? ScheduledTime { get; set; }

        /// <summary>
        /// When this match was started
        /// </summary>
        [JsonPropertyName("started_at")]
        public DateTimeOffset? StartedAt { get; set; }

        /// <summary>
        /// The current state of the match
        /// </summary>
        [JsonPropertyName("state")]
        [JsonConverter(typeof(MatchStateJsonConverter))]
        public MatchState State { get; set; }

        /// <summary>
        /// The id of the tournament this match is attached to
        /// </summary>
        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        /// <summary>
        /// When the match was marked as underway
        /// </summary>
        [JsonPropertyName("underway_at")]
        public DateTimeOffset? UnderwayAt { get; set; }

        /// <summary>
        /// When the match was last updated
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// The id of the participant that won the match
        /// </summary>
        [JsonPropertyName("winner_id")]
        public int? WinnerId { get; set; }

        /// <summary>
        /// Comma separated id with player 1's prerequisite match first (e.g. "241424229,241424230")
        /// </summary>
        [JsonPropertyName("prerequisite_match_ids_csv")]
        public string PrerequisiteMatchIdsCsv { get; set; }

        /// <summary>
        /// Comma separated set/game scores with player 1 score first (e.g. "1-3,3-0,3-2")
        /// </summary>
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
