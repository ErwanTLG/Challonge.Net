using Challonge.Matches;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Challonge
{
    public partial class ChallongeClient
    {
        /// <summary>
        /// This class contains all the functions related to the matches api
        /// </summary>
        public class MatchesHandler
        {
            private readonly string _apiKey;
            private readonly HttpClient _httpClient;

            internal MatchesHandler(string apiKey, HttpClient httpClient)
            {
                _apiKey = apiKey;
                _httpClient = httpClient;
            }

            /// <summary>
            /// Retrieve a tournament's match list. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="state">If provided, only retrieves matches which are in the given state</param>
            /// <param name="participantId">If provided, only retrieves matches that include the specified
            /// participant</param>
            /// <returns>The matches corresponding to the given query</returns>
            public async Task<Match[]> GetMatchesAsync(string tournament, MatchState? state = null, int? participantId = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches.json?api_key={_apiKey}";

                switch (state)
                {
                    case MatchState.Pending:
                        request += "&state=pending";
                        break;
                    case MatchState.Open:
                        request += "&state=open";
                        break;
                    case MatchState.Complete:
                        request += "&state=complete";
                        break;
                }

                if (participantId != null)
                    request += "&participant_id=" + participantId;

                MatchData[] matchDatas = await GetAsync<MatchData[]>(_httpClient, request);
                Match[] matches = new Match[matchDatas.Length];

                for (int i = 0; i < matchDatas.Length; i++)
                    matches[i] = matchDatas[i].Match;

                return matches;
            }

            /// <summary>
            /// Retrieve a single match record for a tournament. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="includeAttachments">Whether or not to include the attachments of the match</param>
            /// <returns></returns>
            public async Task<Match> GetMatchAsync(string tournament, int matchId, bool includeAttachments = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}.json?api_key={_apiKey}";

                MatchData matchData = await GetAsync<MatchData>(_httpClient, request);
                return matchData.Match;
            }

            /// <summary>
            /// Update/submit the score(s) for a match. 
            /// </summary>
            /// <remarks>
            /// If you're updating <paramref name="winnerId"/>, <paramref name="scoresCsv"/> must also be
            /// provided. You may, however, update <paramref name="scoresCsv"/> without providing 
            /// <paramref name="winnerId"/> for live score updates.
            /// If you change the outcome of a completed match, all matches in the bracket that
            /// branch from the updated match will be reset.
            /// </remarks>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="scoresCsv">Comma separated set/game scores with player 1 score first 
            /// (e.g. "1-3,3-0,3-2")</param>
            /// <param name="winnerId">The participant ID of the winner or "0" for tie if applicable (Round Robin
            /// and Swiss).</param>
            /// <param name="player1Votes">Overwrites the number of votes for player 1</param>
            /// <param name="player2Votes">Overwrites the number of votes for player 2</param>
            /// <returns></returns>
            public async Task<Match> UpdateMatchAsync(string tournament, int matchId, string scoresCsv = null,
                int? winnerId = null, int? player1Votes = null, int? player2Votes = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}.json";

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = _apiKey
                };

                if (scoresCsv != null)
                    parameters["match[scores_csv]"] = scoresCsv;

                if (winnerId != null)
                    parameters["match[winner_id]"] = winnerId.ToString();

                if (player1Votes != null)
                    parameters["match[player1_votes]"] = player1Votes.ToString();

                if (player2Votes != null)
                    parameters["match[player2_votes]"] = player2Votes.ToString();

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

                MatchData matchData = await PutAsync<MatchData>(_httpClient, request, content);
                return matchData.Match;
            }

            /// <summary>
            /// Update/submit the score(s) for a match. 
            /// </summary>
            /// <remarks>
            /// If you're updating <see cref="Match.WinnerId"/>, <see cref="Match.ScoresCsv"/> must also be
            /// provided. You may, however, update <see cref="Match.ScoresCsv"/> without providing 
            /// <see cref="Match.WinnerId"/> for live score updates.
            /// If you change the outcome of a completed match, all matches in the bracket that
            /// branch from the updated match will be reset.
            /// </remarks>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="builder">The updated version of the match</param>
            /// <returns></returns>
            public async Task<Match> UpdateMatchAsync(string tournament, int matchId, MatchBuilder builder)
            {
                return await UpdateMatchAsync(tournament, matchId, builder.ScoresCsv, builder.WinnerId, builder.Player1Votes, builder.Player2Votes);
            }

            /// <summary>
            /// Reopens a match that was marked completed, automatically resetting matches that follow it.
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="matchId">The match's unique id</param>
            public async Task ReopenMatchAsync(string tournament, int matchId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/reopen.json";

                await ApiCallAsync(_httpClient, _apiKey, request);
            }

            /// <summary>
            /// Sets <see cref="Match.UnderwayAt"/> to the current time and highlights the match in the bracket
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="matchId">The match's unique id</param>
            public async Task MarkMatchAsUnderwayAsync(string tournament, int matchId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/mark_as_underway.json";

                await ApiCallAsync(_httpClient, _apiKey, request);
            }

            /// <summary>
            /// Clears <see cref="Match.UnderwayAt"/> and unhighlights the match in the bracket 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournamentUrl (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="matchId">The match's unique id</param>
            public async Task UnmarkMatchAsUnderwayAsync(string tournament, int matchId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/mark_as_underway.json";

                await ApiCallAsync(_httpClient, _apiKey, request);
            }
        }
    }
}
