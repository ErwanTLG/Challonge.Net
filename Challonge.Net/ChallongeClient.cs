using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Challonge.Matches;

namespace Challonge
{
    public partial class ChallongeClient
    {
        internal readonly string apiKey;

        internal static readonly HttpClient httpClient = new HttpClient();

        public ChallongeClient(string key)
        {
            apiKey = key ?? throw new ArgumentNullException("key");
        }

        async public Task<Match[]> GetMatchesAsync(string tournament, MatchState? state = null, int? participantId = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches.json?api_key={apiKey}";

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


            string response = await httpClient.GetStringAsync(request);

            MatchData[] matchDatas = JsonSerializer.Deserialize<MatchData[]>(response);
            Match[] matches = new Match[matchDatas.Length];

            for (int i = 0; i < matchDatas.Length; i++)
                matches[i] = matchDatas[i].Match;

            return matches;
        }
    }
}
