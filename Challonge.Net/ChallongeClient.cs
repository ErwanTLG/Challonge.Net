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

        internal readonly HttpClient httpClient = new HttpClient();

        public TournamentHandler Tournaments { get; }

        public ChallongeClient(string key)
        {
            apiKey = key ?? throw new ArgumentNullException("key");

            Tournaments = new TournamentHandler(apiKey, httpClient);
        }

        public ChallongeClient(string key, HttpClient httpClient)
        {
            apiKey = key ?? throw new ArgumentNullException("key");
            this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");

            Tournaments = new TournamentHandler(apiKey, httpClient);
        }
    }
}
