using Challonge.Matches;

namespace Challonge.Participants
{
    public class ParticipantApiResult
    {
        public Participant Participant { get; internal set; }
        public Match[] Matches { get; internal set; }
    }
}
