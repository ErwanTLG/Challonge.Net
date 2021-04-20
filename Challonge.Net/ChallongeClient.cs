using System;
using System.Net.Http;

namespace Challonge
{
    public partial class ChallongeClient
    {
        internal readonly string apiKey;

        internal readonly HttpClient httpClient = new HttpClient();

        public TournamentHandler Tournaments { get; }
        public ParticipantsHandler Participants { get; }
        public MatchesHandler Matches { get; }

        public ChallongeClient(string key)
        {
            apiKey = key ?? throw new ArgumentNullException("key");

            Tournaments = new TournamentHandler(apiKey, httpClient);
            Participants = new ParticipantsHandler(apiKey, httpClient);
            Matches = new MatchesHandler(apiKey, httpClient);
        }

        public ChallongeClient(string key, HttpClient httpClient)
        {
            apiKey = key ?? throw new ArgumentNullException("key");
            this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");

            Tournaments = new TournamentHandler(apiKey, httpClient);
            Participants = new ParticipantsHandler(apiKey, httpClient);
            Matches = new MatchesHandler(apiKey, httpClient);
        }
    }
}
