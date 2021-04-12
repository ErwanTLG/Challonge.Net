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
    }
}
