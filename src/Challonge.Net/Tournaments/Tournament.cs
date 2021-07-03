using Challonge.Participants;
using Challonge.Tournaments.Json;
using System;
using System.Text.Json.Serialization;

namespace Challonge.Tournaments
{
    internal class TournamentData
    {
        [JsonPropertyName("tournament")]
        public Tournament Tournament { get; set; }
    }

    public class Tournament
    {
        /// <summary>
        /// Allow match attachment uploads
        /// </summary>
        [JsonPropertyName("accept_attachments")]
        public bool AcceptAttachments { get; set; }

        /// <summary>
        /// Whether or not verified Challonge users are allowed to report their own matches
        /// </summary>
        [JsonPropertyName("allow_participant_match_reporting")]
        public bool AllowParticipantMatchReporting { get; set; }

        /// <summary>
        /// Whether or not anonymous votes/predictions are enabled
        /// </summary>
        [JsonPropertyName("anonymous_voting")]
        public bool AnonymousVoting { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; } // I'm not sure what type this should be parsed to

        /// <summary>
        /// Length of the participant check-in window in minutes
        /// </summary>
        [JsonPropertyName("check_in_duration")]
        public int? CheckInDuration { get; set; }

        /// <summary>
        /// When the tournament was completed
        /// </summary>
        [JsonPropertyName("completed_at")]
        public DateTimeOffset? CompletedAt { get; set; }

        /// <summary>
        /// When the tournament was created
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Whether or not the tournament was created by an api call
        /// </summary>
        [JsonPropertyName("created_by_api")]
        public bool CreatedByApi { get; set; }

        [JsonPropertyName("credit_capped")]
        public bool CreditCapped { get; set; }

        /// <summary>
        /// Description/instructions displayed above the bracket
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The unique identifier of the game that your tournament is played on
        /// </summary>
        [JsonPropertyName("game_id")]
        public int? GameId { get; set; }

        [JsonPropertyName("group_stages_enabled")]
        public bool GroupStagesEnabled { get; set; }

        /// <summary>
        /// Hide the forum tab on your Challonge page
        /// </summary>
        [JsonPropertyName("hide_forum")]
        public bool HideForum { get; set; }

        /// <summary>
        /// Hide the seed numbers in the bracket
        /// </summary>
        [JsonPropertyName("hide_seeds")]
        public bool HideSeeds { get; set; }

        /// <summary>
        /// Whether or not to include a match between the semifinals losers. (only works with single elimination brackets)
        /// </summary>
        [JsonPropertyName("hold_third_place_match")]
        public bool HoldThirdPlaceMatch { get; set; }

        /// <summary>
        /// Your tournament's unique identifier
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("max_predictions_per_user")]
        public int MaxPredictionsPerUser { get; set; }

        /// <summary>
        /// Your tournament's name/title (max: 60 characters)
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Email registered Challonge participants when matches open up for them
        /// </summary>
        [JsonPropertyName("notify_users_when_matches_open")]
        public bool NotifyUsersWhenMatchesOpen { get; set; }

        /// <summary>
        /// Email registered Challonge participants the results when this tournament ends
        /// </summary>
        [JsonPropertyName("notify_users_when_the_tournament_ends")]
        public bool NotifyUsersWhenTournamentEnds { get; set; }

        /// <summary>
        /// Whether or not challonge hosts a signup page
        /// </summary>
        [JsonPropertyName("open_signup")]
        public bool OpenSignup { get; set; }

        /// <summary>
        /// Current number of participants
        /// </summary>
        [JsonPropertyName("participants_count")]
        public int ParticipantsCount { get; set; }

        [JsonPropertyName("prediction_method")]
        public TournamentPredictionMethod PredictionMethod { get; set; }

        [JsonPropertyName("predictions_opened_at")]
        public DateTimeOffset? PredictionsOpenedAt { get; set; }

        /// <summary>
        /// Hide this tournament from the public browsable index and your profile
        /// </summary>
        [JsonPropertyName("private")]
        public bool IsPrivate { get; set; }

        /// <summary>
        /// An integer between 0 and 100 representing the tournament completion %
        /// </summary>
        [JsonPropertyName("progress_meter")]
        public int ProgressMeter { get; set; }

        /// <summary>
        /// For swiss rounds only: points gained for a bye round
        /// </summary>
        [JsonPropertyName("pts_for_bye")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForBye { get; set; }

        /// <summary>
        /// For swiss rounds only: points gained for a game tie
        /// </summary>
        [JsonPropertyName("pts_for_game_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForGameTie { get; set; }

        /// <summary>
        /// For swiss rounds only: points gained for a game wain
        /// </summary>
        [JsonPropertyName("pts_for_game_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForGameWin { get; set; }

        /// <summary>
        /// For swiss rounds only: points gained for a match tie
        /// </summary>
        [JsonPropertyName("pts_for_match_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForMatchTie { get; set; }

        /// <summary>
        /// For swiss rounds only: points gained for a match win
        /// </summary>
        [JsonPropertyName("pts_for_match_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float PointsForMatchWin { get; set; }

        /// <summary>
        /// When set to true, you can report the winner without the scores
        /// </summary>
        [JsonPropertyName("quick_advance")]
        public bool QuickAdvance { get; set; }

        /// <summary>
        /// For round robin and siss: how the participants are ranked
        /// </summary>
        [JsonPropertyName("ranked_by")]
        [JsonConverter(typeof(TournamentRankingStatsJsonConverter))]
        public TournamentRankingStats RankedBy { get; set; }

        /// <summary>
        /// Whether or not both teams need to report the score before the match is marked as completed
        /// </summary>
        [JsonPropertyName("require_score_agreement")]
        public bool RequireScoreAgreement { get; set; }

        /// <summary>
        /// For round robin only: number of points gainded for a game tie
        /// </summary>
        [JsonPropertyName("rr_pts_for_game_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForGameTie { get; set; }

        /// <summary>
        /// For round robin only: number of points gainded for a game win
        /// </summary>
        [JsonPropertyName("rr_pts_for_game_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForGameWin { get; set; }

        /// <summary>
        /// For round robin only: number of points gainded for a match tie
        /// </summary>
        [JsonPropertyName("rr_pts_for_match_tie")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForMatchTie { get; set; }

        /// <summary>
        /// For round robin only: number of points gainded for a match win
        /// </summary>
        [JsonPropertyName("rr_pts_for_match_win")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float RoundRobinPointsForMatchWin { get; set; }

        /// <summary>
        /// When enabled, instead of traditional seeding rules, make pairings by going straight
        /// down the list of participants. First round matches are filled in top to bottom, 
        /// then qualifying matches (if applicable)
        /// </summary>
        [JsonPropertyName("sequential_pairings")]
        public bool SequentialPairing { get; set; }

        /// <summary>
        /// Label each round above the bracket (only works with single and double elimination brackets)
        /// </summary>
        [JsonPropertyName("show_rounds")]
        public bool ShowRounds { get; set; }

        /// <summary>
        /// Max number of participants in your tournament
        /// </summary>
        /// <remarks>
        /// Once this number is reached, participant will be added to a waiting list. You can check if a
        /// participant is in the waiting list using <see cref="Participant.OnWaitingList"/>
        /// </remarks>
        [JsonPropertyName("signup_cap")]
        public int? SignupCap { get; set; }

        /// <summary>
        /// When the tournament is scheduled to start
        /// </summary>
        [JsonPropertyName("start_at")]
        public DateTimeOffset? StartAt { get; set; }

        /// <summary>
        /// When the tournament has started
        /// </summary>
        [JsonPropertyName("started_at")]
        public DateTimeOffset? StartedAt { get; set; }

        /// <summary>
        /// When the check-in started
        /// </summary>
        [JsonPropertyName("started_checking_in_at")]
        public DateTimeOffset? StartedCheckingInAt { get; set; }

        /// <summary>
        /// Current state of the tournament
        /// </summary>
        [JsonPropertyName("state")]
        [JsonConverter(typeof(TournamentStateJsonConverter))]
        public TournamentState State { get; set; }

        /// <summary>
        /// Number of rounds to be played. Only works for swiss type tournaments.
        /// </summary>
        [JsonPropertyName("swiss_rounds")]
        public int SwissRounds { get; set; }

        /// <summary>
        /// Whether or not teams are enabled
        /// </summary>
        [JsonPropertyName("teams")]
        public bool? Teams { get; set; }

        /// <summary>
        /// How to handle ties: sorted by descending priority
        /// </summary>
        [JsonPropertyName("tie_breaks")]
        [JsonConverter(typeof(TournamentTieBreakArrayJsonConverter))]
        public TournamentTieBreak[] TieBreaks { get; set; }

        /// <summary>
        /// The type of the tournament
        /// </summary>
        [JsonPropertyName("tournament_type")]
        [JsonConverter(typeof(TournamentTypeJsonConverter))]
        public TournamentType TournamentType { get; set; }

        /// <summary>
        /// When the tournament was last updated
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Your tournament's url (challonge.com/url)
        /// letters, numbers, and underscores only
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("description_source")]
        public string DescriptionSource { get; set; }

        /// <summary>
        /// Your tournament's subdomain (subdomain.challonge.com/url)
        /// letters, numbers, and underscores only
        /// </summary>
        [JsonPropertyName("subdomain")]
        public string Subdomain { get; set; }

        /// <summary>
        /// The tournament's url
        /// </summary>
        [JsonPropertyName("full_challonge_url")]
        public string FullChallongeUrl { get; set; }

        [JsonPropertyName("live_image_url")]
        public string LiveImageUrl { get; set; }

        /// <summary>
        /// Signup page's url
        /// </summary>
        [JsonPropertyName("sign_up_url")]
        public string SignupUrl { get; set; }

        [JsonPropertyName("review_before_finalizing")]
        public bool ReviewBeforeFinalizing { get; set; }

        /// <summary>
        /// Whether or not the predictions are enabled
        /// </summary>
        [JsonPropertyName("accepting_predictions")]
        public bool AcceptingPredictions { get; set; }

        [JsonPropertyName("participants_locked")]
        public bool ParticipantsLocked { get; set; }

        /// <summary>
        /// The name of the game your tournament is played on
        /// </summary>
        [JsonPropertyName("game_name")]
        public string GameName { get; set; }

        [JsonPropertyName("participants_swappable")]
        public bool ParticipantsSwappable { get; set; }

        [JsonPropertyName("team_convertable")]
        public bool TeamConvertable { get; set; }

        /// <summary>
        /// Two stage tournaments only: whether or not the group stage were started
        /// </summary>
        [JsonPropertyName("group_stages_were_started")]
        public bool GroupStageWereStarted { get; set; }

        /// <summary>
        /// How the grand finals are played
        /// </summary>
        [JsonPropertyName("grand_finals_modifier")]
        [JsonConverter(typeof(TournamentGrandFinalsJsonConverter))]
        public TournamentGrandFinals GrandFinalsModifier { get; set; }
    }

    public enum TournamentType
    {
        SingleElimination,
        DoubleElimination,
        RoundRobin,
        Swiss,
        TimeTrial,
        SingleRace,
        GrandPrix,
        FreeForAll,
        Invalid
    }

    public enum TournamentState
    {
        /// <summary>
        /// The check-in process has started
        /// </summary>
        CheckingIn,

        /// <summary>
        /// The check-in process is completed
        /// </summary>
        CheckedIn,

        /// <summary>
        /// The tournament has not started yet
        /// </summary>
        Pending,

        /// <summary>
        /// The tournament has started
        /// </summary>
        Underway,
        
        /// <summary>
        /// The tournament is completed
        /// </summary>
        Completed,
        Invalid
    }

    public enum TournamentPredictionMethod
    {
        Exponential = 1,
        Linear = 2,
        Invalid
    }

    public enum TournamentRankingStats
    {
        MatchWins,
        GameWins,
        GameWinPercentage,
        PointsScored,
        PointsDifference,
        Custom,
        Invalid
    }

    public enum TournamentTieBreak
    {
        MatchWins,
        GameWins,
        GameWinPercentage,
        PointsScored,
        PointsDifference,
        WinsVSTiedParticipants,
        MedianBuchholz,
        Invalid
    }

    public enum TournamentGrandFinals
    {
        TwoChances,
        SingleMatch,
        Skip
    }
}
