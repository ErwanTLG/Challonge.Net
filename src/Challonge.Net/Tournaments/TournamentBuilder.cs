using System;

namespace Challonge.Tournaments
{
    public class TournamentBuilder
    {
        /// <summary>
        /// Allow match attachment uploads
        /// </summary>
        public bool AcceptAttachments { get; set; }

        /// <summary>
        /// Length of the participant check-in window in minutes
        /// </summary>
        public int? CheckInDuration { get; set; }

        /// <summary>
        /// Description/instructions displayed above the bracket
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// How the grand finals are played
        /// </summary>
        public TournamentGrandFinals GrandFinalsModifier { get; set; } = TournamentGrandFinals.TwoChances;

        /// <summary>
        /// Hide the forum tab on your Challonge page
        /// </summary>
        public bool HideForum { get; set; }

        /// <summary>
        /// Whether or not to include a match between the semifinals losers. (only works with single elimination brackets)
        /// </summary>
        public bool HoldThirdPlaceMatch { get; set; }

        /// <summary>
        /// Hide this tournament from the public browsable index and your profile
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Your tournament's name/title (max: 60 characters)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email registered Challonge participants when matches open up for them
        /// </summary>
        public bool NotifyUsersWhenMatchesOpen { get; set; }

        /// <summary>
        /// Email registered Challonge participants the results when this tournament ends
        /// </summary>
        public bool NotifyUsersWhenTournamentEnds { get; set; }

        /// <summary>
        /// Whether or not challonge hosts a signup page for you
        /// </summary>
        public bool OpenSignup { get; set; }

        /// <summary>
        /// For swiss rounds only: points gained for a bye round
        /// </summary>
        public float PointsForBye { get; set; } = 1f;

        /// <summary>
        /// Points gained for a game tie
        /// </summary>
        public float PointsForGameTie { get; set; }

        /// <summary>
        /// Points gained for a game win
        /// </summary>
        public float PointsForGameWin { get; set; }

        /// <summary>
        /// Points gained for a match tie
        /// </summary>
        public float PointsForMatchTie { get; set; } = 0.5f;

        /// <summary>
        /// Points gained for a match win
        /// </summary>
        public float PointsForMatchWin { get; set; } = 1f;

        /// <summary>
        /// For round robin and swiss: how the participants are ranked
        /// </summary>
        public TournamentRankingStats RankedBy { get; set; } = TournamentRankingStats.MatchWins;

        /// <summary>
        /// When enabled, instead of traditional seeding rules, make pairings by going straight
        /// down the list of participants. First round matches are filled in top to bottom,
        /// then qualifying matches (if applicable)
        /// </summary>
        public bool SequentialPairings { get; set; }

        /// <summary>
        /// Max number of participants in your tournament
        /// </summary>
        /// <remarks>
        /// Once this number is reached, participant will be added to a waiting list. You can check if a
        /// participant is in the waiting list using <see cref="Participants.Participant.OnWaitingList" />
        /// </remarks>
        public int? SignupCap { get; set; }

        /// <summary>
        /// Label each round above the bracket (only works with single and double elimination brackets)
        /// </summary>
        public bool ShowRounds { get; set; }

        /// <summary>
        /// When the tournament is scheduled to start
        /// </summary>
        public DateTimeOffset? StartAt { get; set; }

        /// <summary>
        /// Your tournament's subdomain (subdomain.challonge.com/url)
        /// letters, numbers, and underscores only
        /// </summary>
        public string Subdomain { get; set; }

        //TODO try making null
        /// <summary>
        /// Number of rounds to be played. Only works for swiss type tournaments.
        /// </summary>
        /// <remarks>
        /// It is recommend to limit the number of rounds to less than two-thirds the number of players.
        /// Otherwise, an impossible pairing situation can be reached and your tournament may end before
        /// the desired number of rounds are played.
        /// </remarks>
        public int? SwissRounds { get; set; }

        /// <summary>
        /// The type of the tournament
        /// </summary>
        public TournamentType TournamentType { get; set; } = TournamentType.SingleElimination;

        /// <summary>
        /// Your tournament's url (challonge.com/url)
        /// letters, numbers, and underscores only
        /// </summary>
        public string Url { get; set; }

        public TournamentBuilder() { }
        
        public TournamentBuilder(Tournament tournament)
        {
            AcceptAttachments = tournament.AcceptAttachments;
            CheckInDuration = tournament.CheckInDuration;
            Description = tournament.Description;
            GrandFinalsModifier = tournament.GrandFinalsModifier;
            HideForum = tournament.HideForum;
            HoldThirdPlaceMatch = tournament.HoldThirdPlaceMatch;
            IsPrivate = tournament.IsPrivate;
            Name = tournament.Name;
            NotifyUsersWhenMatchesOpen = tournament.NotifyUsersWhenMatchesOpen;
            NotifyUsersWhenTournamentEnds = tournament.NotifyUsersWhenTournamentEnds;
            OpenSignup = tournament.OpenSignup;
            PointsForBye = tournament.PointsForBye;
            RankedBy = RankedBy;
            SequentialPairings = SequentialPairings;
            SignupCap = SignupCap;
            ShowRounds = ShowRounds;
            StartAt = StartAt;
            Subdomain = Subdomain;
            SwissRounds = SwissRounds;
            TournamentType = tournament.TournamentType;
            Url = Url;
            
            if (tournament.TournamentType == TournamentType.RoundRobin)
            {
                PointsForGameTie = tournament.RoundRobinPointsForGameTie;
                PointsForGameWin = tournament.RoundRobinPointsForGameWin;
                PointsForMatchTie = tournament.RoundRobinPointsForMatchTie;
                PointsForMatchWin = tournament.RoundRobinPointsForMatchWin;
            }
            else
            {
                PointsForGameTie = PointsForGameTie;
                PointsForGameWin = PointsForGameWin;
                PointsForMatchTie = PointsForMatchTie;
                PointsForMatchWin = PointsForMatchWin;
            }
        }
        
        /// <summary>
        /// Creates the <see cref="Tournament" /> with the corresponding properties
        /// </summary>
        /// <returns></returns>
        public Tournament ToTournament()
        {
            if (string.IsNullOrEmpty(Name))
                throw new NullReferenceException("Name can't be blank");

            Tournament tournament = new Tournament
            {
                AcceptAttachments = AcceptAttachments,
                CheckInDuration = CheckInDuration,
                Description = Description,
                GrandFinalsModifier = GrandFinalsModifier,
                HideForum = HideForum,
                HoldThirdPlaceMatch = HoldThirdPlaceMatch,
                IsPrivate = IsPrivate,
                Name = Name,
                NotifyUsersWhenMatchesOpen = NotifyUsersWhenMatchesOpen,
                NotifyUsersWhenTournamentEnds = NotifyUsersWhenTournamentEnds,
                OpenSignup = OpenSignup,
                PointsForBye = PointsForBye,
                PointsForGameTie = PointsForGameTie,
                PointsForGameWin = PointsForGameWin,
                PointsForMatchTie = PointsForMatchTie,
                PointsForMatchWin = PointsForMatchWin,
                RoundRobinPointsForGameTie = PointsForGameTie,
                RoundRobinPointsForGameWin = PointsForGameWin,
                RoundRobinPointsForMatchTie = PointsForMatchTie,
                RoundRobinPointsForMatchWin = PointsForMatchWin,
                RankedBy = RankedBy,
                SequentialPairing = SequentialPairings,
                SignupCap = SignupCap,
                ShowRounds = ShowRounds,
                StartAt = StartAt,
                Subdomain = Subdomain,
                TournamentType = TournamentType,
                Url = Url
            };

            if (SwissRounds != null)
                tournament.SwissRounds = SwissRounds.Value;

            return tournament;
        }
    }
}