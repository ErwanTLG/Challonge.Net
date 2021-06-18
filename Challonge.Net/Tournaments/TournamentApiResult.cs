using Challonge.Matches;
using Challonge.Participants;
using Challonge.Tournaments;

namespace Challonge
{
    /// <summary>
    /// This class contains the result of an API call.
    /// </summary>
    public class TournamentApiResult
    {
        /// <summary>
        /// The tournament that was retrieved
        /// </summary>
        public Tournament Tournament { get; internal set; }

        /// <summary>
        /// The matches attached to the tournament
        /// </summary>
        /// <remarks>Will be null if includeMatches was set to false</remarks>
        public Match[] Matches { get; internal set; }

        /// <summary>
        /// The participants attached to the tournament
        /// </summary>
        /// <remarks>Will be null if includeParticipants was set to false</remarks>
        public Participant[] Participants { get; internal set; }
    }
}
