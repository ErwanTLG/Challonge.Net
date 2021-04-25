using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Challonge
{
    public partial class ChallongeClient
    {
        internal readonly string apiKey;

        internal readonly HttpClient httpClient = new HttpClient();

        public TournamentHandler Tournaments { get; }
        public ParticipantsHandler Participants { get; }
        public MatchesHandler Matches { get; }
        public AttachmentsHandler Attachments
        {
            get;
        }
        public ChallongeClient(string key)
        {
            apiKey = key ?? throw new ArgumentNullException("key");

            Tournaments = new TournamentHandler(apiKey, httpClient);
            Participants = new ParticipantsHandler(apiKey, httpClient);
            Matches = new MatchesHandler(apiKey, httpClient);
            Attachments = new AttachmentsHandler(apiKey, httpClient);
        }

        public ChallongeClient(string key, HttpClient httpClient)
        {
            apiKey = key ?? throw new ArgumentNullException("key");
            this.httpClient = httpClient ?? throw new ArgumentNullException("httpClient");

            Tournaments = new TournamentHandler(apiKey, httpClient);
            Participants = new ParticipantsHandler(apiKey, httpClient);
            Matches = new MatchesHandler(apiKey, httpClient);
            Attachments = new AttachmentsHandler(apiKey, httpClient);
        }

        private static async Task<T> GetAsync<T>(HttpClient client, string request)
        {
            HttpResponseMessage response = await client.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            byte[] byteString = Encoding.UTF8.GetBytes(responseString);
            MemoryStream reader = new MemoryStream(byteString);

            T result = await JsonSerializer.DeserializeAsync<T>(reader);
            return result;
        }

        private static async Task<T> PostAsync<T>(HttpClient client, string request, HttpContent content)
        {
            HttpResponseMessage response = await client.PostAsync(request, content);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            byte[] byteString = Encoding.UTF8.GetBytes(responseString);
            MemoryStream reader = new MemoryStream(byteString);

            T result = await JsonSerializer.DeserializeAsync<T>(reader);
            return result;
        }

        private static async Task<T> PutAsync<T>(HttpClient client, string request, HttpContent content)
        {
            HttpResponseMessage response = await client.PutAsync(request, content);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            byte[] byteString = Encoding.UTF8.GetBytes(responseString);
            MemoryStream reader = new MemoryStream(byteString);

            T result = await JsonSerializer.DeserializeAsync<T>(reader);
            return result;
        }

        private static async Task DeleteAsync(HttpClient client, string request)
        {
            HttpResponseMessage response = await client.DeleteAsync(request);
            await ErrorHandler.ParseResponseAsync(response);
        }

        private static async Task ApiCallAsync(HttpClient client, string apiKey, string request)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            { ["api_key"] = apiKey };

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            await client.PostAsync(request, content);
        }
    }
}
