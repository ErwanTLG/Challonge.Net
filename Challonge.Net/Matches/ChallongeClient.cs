using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Challonge.Matches;

namespace Challonge
{
    public partial class ChallongeClient
    {
        private async Task<Match> MatchApiCallAsync(string request)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PostAsync(request, content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData matchData = JsonSerializer.Deserialize<MatchData>(responseString);

            return matchData.Match;
        }

        public async Task<Match[]> GetMatchesAsync(int tournamentId, MatchState? state = null, int? participantId = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/matches.json?api_key={apiKey}";

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


            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData[] matchDatas = JsonSerializer.Deserialize<MatchData[]>(responseString);
            Match[] matches = new Match[matchDatas.Length];

            for (int i = 0; i < matchDatas.Length; i++)
                matches[i] = matchDatas[i].Match;

            return matches;
        }

        public async Task<Match[]> GetMatchesAsync(string tournamentUrl, MatchState? state = null, int? participantId = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/matches.json?api_key={apiKey}";

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


            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData[] matchDatas = JsonSerializer.Deserialize<MatchData[]>(responseString);
            Match[] matches = new Match[matchDatas.Length];

            for (int i = 0; i < matchDatas.Length; i++)
                matches[i] = matchDatas[i].Match;

            return matches;
        }

        // TODO handle attachements
        public async Task<Match> GetMatchAsync(string tournamentUrl, int matchId, bool includeAttachments = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/matches/{matchId}.json?api_key={apiKey}";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData matchData = JsonSerializer.Deserialize<MatchData>(responseString);

            return matchData.Match;
        }

        public async Task<Match> GetMatchAsync(int tournamentId, int matchId, bool includeAttachments = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/matches/{matchId}.json?api_key={apiKey}";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData matchData = JsonSerializer.Deserialize<MatchData>(responseString);

            return matchData.Match;
        }

        public async Task<Match> UpdateMatchAsync(string tournamentUrl, int matchId, string scoresCsv = null, 
            int? winnerId = null, int? player1Votes = null, int? player2Votes = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
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
            HttpResponseMessage response = await httpClient.PutAsync(
                $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/matches/{matchId}.json", content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData matchData = JsonSerializer.Deserialize<MatchData>(responseString);
            return matchData.Match;
        }

        public async Task<Match> UpdateMatchAsync(int tournamentId, int matchId, string scoresCsv = null,
            int? winnerId = null, int? player1Votes = null, int? player2Votes = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
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
            HttpResponseMessage response = await httpClient.PutAsync(
                $"https://api.challonge.com/v1/tournaments/{tournamentId}/matches/{matchId}.json", content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            MatchData matchData = JsonSerializer.Deserialize<MatchData>(responseString);
            return matchData.Match;
        }

        public async Task<Match> UpdateMatchAsync(string tournamentUrl, Match match)
        {
            return await UpdateMatchAsync(tournamentUrl, match.Id, match.ScoresCsv, match.WinnerId, match.Player1Votes, match.Player2Votes);
        }

        public async Task<Match> UpdateMatchAsync(int tournamentId, Match match)
        {
            return await UpdateMatchAsync(tournamentId, match.Id, match.ScoresCsv, match.WinnerId, match.Player1Votes, match.Player2Votes);
        }

        public async Task<Match> ReopenMatchAsync(string tournamentUrl, int matchId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/matches/{matchId}/reopen.json";

            return await MatchApiCallAsync(request);
        }

        public async Task<Match> ReopenMatchAsync(int tournamentId, int matchId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/matches/{matchId}/reopen.json";

            return await MatchApiCallAsync(request);
        }

        public async Task<Match> MarkMatchAsUnderwayAsync(string tournamentUrl, int matchId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/matches/{matchId}/mark_as_underway.json";

            return await MatchApiCallAsync(request);
        }

        public async Task<Match> MarkMatchAsUnderwayAsync(int tournamentId, int matchId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/matches/{matchId}/mark_as_underway.json";

            return await MatchApiCallAsync(request);
        }

        public async Task<Match> UnmarkMatchAsUnderwayAsync(string tournamentUrl, int matchId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/matches/{matchId}/mark_as_underway.json";

            return await MatchApiCallAsync(request);
        }

        public async Task<Match> UnmarkMatchAsUnderwayAsync(int tournamentId, int matchId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/matches/{matchId}/mark_as_underway.json";

            return await MatchApiCallAsync(request);
        }
    }
}
