using System;
using System.Text.Json.Serialization;
using Challonge.Tournaments.Json;

namespace Challonge.Tournaments
{
    internal class TournamentData
    {
        [JsonPropertyName("tournament")]
        public Tournament Tournament { get; set; }
    }

    public class Tournament
    {
        [JsonPropertyName("accept_attachments")]
        public bool AcceptAttachments { get; set; }

        [JsonPropertyName("allow_participant_match_reporting")]
        public bool AllowParticipantMatchReporting { get; internal set; }

        [JsonPropertyName("anonymous_voting")]
        public bool AnonymousVoting { get; internal set; }

        [JsonPropertyName("category")]
        public string Category { get; internal set; }   // I'm not sure what type this should be parsed to

        [JsonPropertyName("check_in_duration")]
        public int? CheckInDuration { get; set; }

        [JsonPropertyName("completed_at")]
        public DateTimeOffset? CompletedAt { get; internal set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; internal set; }

        [JsonPropertyName("created_by_api")]
        public bool CreatedByApi { get; internal set; }

        [JsonPropertyName("credit_capped")]
        public bool CreditCapped { get; internal set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("game_id")]
        public int? GameId { get; internal set; }

        [JsonPropertyName("group_stages_enabled")]
        public bool GroupStagesEnabled { get; internal set; }

        [JsonPropertyName("hide_forum")]
        public bool HideForum { get; set; }

        [JsonPropertyName("hide_seeds")]
        public bool HideSeeds { get; internal set; }

        [JsonPropertyName("hold_third_place_match")]
        public bool HoldThirdPlaceMatch { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; internal set; }

        [JsonPropertyName("max_predictions_per_user")]
        public int MaxPredictionsPerUser { get; internal set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("notify_users_when_matches_open")]
        public bool NotifyUsersWhenMatchesOpen { get; set; }

        [JsonPropertyName("notify_users_when_the_tournament_ends")]
        public bool NotifyUsersWhenTournamentEnds { get; set; }

        [JsonPropertyName("open_signup")]
        public bool OpenSignup { get; set; }

        [JsonPropertyName("participants_count")]
        public int ParticipantsCount { get; internal set; }

        [JsonPropertyName("prediction_method")]
        public int PredictionMethod { get; internal set; }    // not sure

        [JsonPropertyName("predictions_opened_at")]
        public DateTimeOffset? PredictionsOpenedAt { get; internal set; }

        [JsonPropertyName("private")]
        public bool IsPrivate { get; set; }

        [JsonPropertyName("progress_meter")]
        public int ProgressMeter { get; internal set;  }

        [JsonPropertyName("pts_for_bye")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForBye { get; set; }

        [JsonPropertyName("pts_for_game_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForGameTie { get; set; }

        [JsonPropertyName("pts_for_game_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForGameWin { get; set; }

        [JsonPropertyName("pts_for_match_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForMatchTie { get; set; }

        [JsonPropertyName("pts_for_match_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForMatchWin { get; set; }

        [JsonPropertyName("quick_advance")]
        public bool QuickAdvance { get; internal set; }

        [JsonPropertyName("ranked_by")]
        public string RankedBy { get; set; }     // not sure (enum maybe)

        [JsonPropertyName("require_score_agreement")]
        public bool RequireScoreAgreement { get; internal set; }

        [JsonPropertyName("rr_pts_for_game_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForGameTie { get; set; }

        [JsonPropertyName("rr_pts_for_game_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForGameWin { get; set; }

        [JsonPropertyName("rr_pts_for_match_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForMatchTie { get; set; }

        [JsonPropertyName("rr_pts_for_match_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForMatchWin { get; set; }

        [JsonPropertyName("sequential_pairings")]
        public bool SequentialPairing { get; set; }

        [JsonPropertyName("show_rounds")]
        public bool ShowRounds { get; set; }

        [JsonPropertyName("signup_cap")]
        public int? SignupCap { get; set; }      // not sure

        [JsonPropertyName("start_at")]
        public DateTimeOffset? StartAt { get; set; }

        [JsonPropertyName("started_at")]
        public DateTimeOffset? StartedAt { get; internal set; }

        [JsonPropertyName("started_checking_in_at")]
        public DateTimeOffset? StartedCheckingInAt { get; internal set; }

        [JsonPropertyName("state")]
        public DateTimeOffset State { get; internal set; }

        [JsonPropertyName("swiss_rounds")]
        public int SwissRounds { get; set; }

        [JsonPropertyName("teams")]
        public bool? Teams { get; internal set; }

        [JsonPropertyName("tie_breaks")]
        public string[] TieBreaks { get; internal set; }  // stack or queue of enums or something like that

        [JsonPropertyName("tournament_type")]
        [JsonConverter(typeof(TournamentTypeJsonConverter))]
        public TournamentType TournamentType { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; internal set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("description_source")]
        public string DescriptionSource { get; internal set; }

        [JsonPropertyName("subdomain")]
        public string Subdomain { get; set; }

        [JsonPropertyName("full_challonge_url")]
        public string FullChallongeUrl { get; internal set; }

        [JsonPropertyName("live_image_url")]
        public string LiveImageUrl { get; internal set; }

        [JsonPropertyName("sign_up_url")]
        public string SignupUrl { get; internal set; }

        [JsonPropertyName("review_before_finalizing")]
        public bool ReviewBeforeFinalizing { get; internal set; }

        [JsonPropertyName("accepting_predictions")]
        public bool AcceptingPredictions { get; internal set; }

        [JsonPropertyName("participants_locked")]
        public bool ParticipantsLocked { get; internal set; }

        [JsonPropertyName("game_name")]
        public string GameName { get; internal set; }

        [JsonPropertyName("participants_swappable")]
        public bool ParticipantsSwappable { get; internal set; }

        [JsonPropertyName("team_convertable")]
        public bool TeamConvertable { get; internal set; }

        [JsonPropertyName("group_stages_were_started")]
        public bool GroupStageWereStarted { get; internal set; }
    }

    public enum TournamentType
    {
        Invalid,
        SingleElimination,
        DoubleElimination,
        RoundRobin,
        Swiss
    }

    public enum TournamentState
    {
        Invalid,
        Pending,
        InProgress,
        Ended
    }
}
