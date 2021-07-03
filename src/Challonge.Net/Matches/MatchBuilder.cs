namespace Challonge.Matches
{
    public class MatchBuilder
    {
        /// <summary>
        /// Comma separated set/game scores with player 1 score first (e.g. "1-3,3-0,3-2")
        /// </summary>
        public string ScoresCsv { get; set; }

        /// <summary>
        /// The participant ID of the winner or "0" for tie if applicable (Round Robin and Swiss).
        /// </summary>
        public int? WinnerId { get; set; }

        /// <summary>
        /// Overwrites the number of votes for player 1
        /// </summary>
        public int? Player1Votes { get; set; }

        /// <summary>
        /// Overwrites the number of votes for player 2
        /// </summary>
        public int? Player2Votes { get; set; }
        
        public MatchBuilder() { }
        
        public MatchBuilder(Match match)
        {
            ScoresCsv = match.ScoresCsv;
            WinnerId = match.WinnerId;
            Player1Votes = match.Player1Votes;
            Player2Votes = match.Player2Votes;
        }

        /// <summary>
        /// Creates the <see cref="Match"/> with the corresponding properties
        /// </summary>
        /// <returns></returns>
        public Match ToMatch()
        {
            return new Match
            {
                ScoresCsv = ScoresCsv,
                WinnerId = WinnerId,
                Player1Votes = Player1Votes,
                Player2Votes = Player2Votes
            };
        }
    }
}