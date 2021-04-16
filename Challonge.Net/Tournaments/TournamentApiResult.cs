using Challonge.Tournaments;
using Challonge.Participants;
using Challonge.Matches;

namespace Challonge
{
    public class TournamentApiResult
    {
        public Tournament Tournament { get; internal set; }
        public Match[] Matches { get; internal set; }
        public Participant[] Participants { get; internal set; }
    }
}
