using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Challonge.Participants;
using Challonge.Matches;

namespace Challonge
{
    public partial class ChallongeClient
    {
        public async Task<Participant[]> GetParticipantsAsync(string tournamentUrl)
        {
            string request = $" https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants.json";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantData[] participantDatas = JsonSerializer.Deserialize<ParticipantData[]>(responseString);
            Participant[] participants = new Participant[participantDatas.Length];

            for (int i = 0; i < participantDatas.Length; i++)
                participants[i] = participantDatas[i].Participant;

            return participants;
        }

        public async Task<Participant[]> GetParticipantsAsync(int tournamentId)
        {
            string request = $" https://api.challonge.com/v1/tournaments/{tournamentId}/participants.json";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantData[] participantDatas = JsonSerializer.Deserialize<ParticipantData[]>(responseString);
            Participant[] participants = new Participant[participantDatas.Length];

            for (int i = 0; i < participantDatas.Length; i++)
                participants[i] = participantDatas[i].Participant;

            return participants;
        }

        public async Task<Participant> CreateParticipantAsync(string tournamentUrl, string name = null,
            string challongeUsername = null, string email = null, int? seed = null, string misc = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants.json";

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
            };

            if (name != null)
                parameters["participant[name]"] = name;

            if (challongeUsername != null)
                parameters["participant[challonge_username]"] = challongeUsername;

            if (email != null)
                parameters["participant[email]"] = email;

            if (seed != null)
                parameters["participant[seed]"] = seed.ToString();

            if (misc != null)
                parameters["participant[misc]"] = misc;

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PostAsync(request, content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantData participantData = JsonSerializer.Deserialize<ParticipantData>(responseString);
            return participantData.Participant;
        }

        public async Task<Participant> CreateParticipantAsync(int tournamentId, string name = null,
            string challongeUsername = null, string email = null, int? seed = null, string misc = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/participants.json";

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
            };

            if (name != null)
                parameters["participant[name]"] = name;

            if (challongeUsername != null)
                parameters["participant[challonge_username]"] = challongeUsername;

            if (email != null)
                parameters["participant[email]"] = email;

            if (seed != null)
                parameters["participant[seed]"] = seed.ToString();

            if (misc != null)
                parameters["participant[misc]"] = misc;

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PostAsync(request, content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantData participantData = JsonSerializer.Deserialize<ParticipantData>(responseString);
            return participantData.Participant;
        }

        public async Task<Participant> CreateParticipantAsync(string tournamentUrl, Participant participant)
        {
            return await CreateParticipantAsync(tournamentUrl, participant.Name, participant.ChallongeUsername,
                participant.InviteEmail, participant.Seed, participant.Misc);
        }

        public async Task<Participant> CreateParticipantAsync(int tournamentId, Participant participant)
        {
            return await CreateParticipantAsync(tournamentId, participant.Name, participant.ChallongeUsername,
                participant.InviteEmail, participant.Seed, participant.Misc);
        }

        //TODO add bulk add

        public async Task<ParticipantApiResult> GetParticipantAsync(string tournamentUrl, int participantId, bool includeMatches = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants/{participantId}.json?api_key={apiKey}";

            if (includeMatches)
                request += "&include_matches=1";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantApiResult result = new ParticipantApiResult();

            if (includeMatches)
            {
                JsonElement matchesElement = JsonDocument.Parse(responseString).RootElement.GetProperty("participant").GetProperty("matches");
                MatchData[] matches = JsonSerializer.Deserialize<MatchData[]>(matchesElement.ToString());
                Match[] matchesResult = new Match[matches.Length];

                for (int i = 0; i < matches.Length; i++)
                    matchesResult[i] = matches[i];

                result.Matches = matchesResult;
            }
            else
                result.Matches = null;

            ParticipantData participantData = JsonSerializer.Deserialize<ParticipantData>(responseString);
            result.Participant = participantData.Participant;

            return result;
        }

        public async Task<ParticipantApiResult> GetParticipantAsync(int tournamentId, int participantId, bool includeMatches = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/participants/{participantId}.json?api_key={apiKey}";

            if (includeMatches)
                request += "&include_matches=1";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantApiResult result = new ParticipantApiResult();

            if (includeMatches)
            {
                JsonElement matchesElement = JsonDocument.Parse(responseString).RootElement.GetProperty("participant").GetProperty("matches");
                MatchData[] matches = JsonSerializer.Deserialize<MatchData[]>(matchesElement.ToString());
                Match[] matchesResult = new Match[matches.Length];

                for (int i = 0; i < matches.Length; i++)
                    matchesResult[i] = matches[i];

                result.Matches = matchesResult;
            }
            else
                result.Matches = null;

            ParticipantData participantData = JsonSerializer.Deserialize<ParticipantData>(responseString);
            result.Participant = participantData.Participant;

            return result;
        }

        public async Task<Participant> UpdateParticipantAsync(string tournamentUrl, int participantId, string name = null,
            string challongeUsername = null, string email = null, int? seed = null, string misc = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants{participantId}.json";

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
            };

            if (name != null)
                parameters["participant[name]"] = name;

            if (challongeUsername != null)
                parameters["participant[challonge_username]"] = challongeUsername;

            if (email != null)
                parameters["participant[email]"] = email;

            if (seed != null)
                parameters["participant[seed]"] = seed.ToString();

            if (misc != null)
                parameters["participant[misc]"] = misc;

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PutAsync(request, content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantData participantData = JsonSerializer.Deserialize<ParticipantData>(responseString);
            return participantData.Participant;
        }

        public async Task<Participant> UpdateParticipantAsync(int tournamentId, int participantId, string name = null,
            string challongeUsername = null, string email = null, int? seed = null, string misc = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentId}/participants/{participantId}.json";

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
            };

            if (name != null)
                parameters["participant[name]"] = name;

            if (challongeUsername != null)
                parameters["participant[challonge_username]"] = challongeUsername;

            if (email != null)
                parameters["participant[email]"] = email;

            if (seed != null)
                parameters["participant[seed]"] = seed.ToString();

            if (misc != null)
                parameters["participant[misc]"] = misc;

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PutAsync(request, content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            ParticipantData participantData = JsonSerializer.Deserialize<ParticipantData>(responseString);
            return participantData.Participant;
        }

        public async Task<Participant> UpdateParticipantAsync(string tournamentUrl, Participant participant)
        {
            return await UpdateParticipantAsync(tournamentUrl, participant.Id, participant.Name,
                participant.ChallongeUsername, participant.InviteEmail, participant.Seed, participant.Misc);
        }

        public async Task<Participant> UpdateParticipantAsync(int tournamentId, Participant participant)
        {
            return await UpdateParticipantAsync(tournamentId, participant.Id, participant.Name,
                participant.ChallongeUsername, participant.InviteEmail, participant.Seed, participant.Misc);
        }

        public async Task CheckInAsync(string tournamentUrl, int participantId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants/{participantId}/check_in.json";

            KeyValuePair<string, string> key = new KeyValuePair<string, string>("api_key", apiKey);

            FormUrlEncodedContent content = new FormUrlEncodedContent(key);
            await httpClient.PutAsync(request, content);
        }

        public async Task UndoCheckInAsync(string tournamentUrl, int participantId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants/{participantId}/undo_check_in.json";

            KeyValuePair<string, string> key = new KeyValuePair<string, string>("api_key", apiKey);

            FormUrlEncodedContent content = new FormUrlEncodedContent(key);
            await httpClient.PutAsync(request, content);
        }

        public async Task DeleteParticipantAsync(string tournamentUrl, int participantId)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants/{participantId}.json";

            KeyValuePair<string, string> key = new KeyValuePair<string, string>("api_key", apiKey);

            FormUrlEncodedContent content = new FormUrlEncodedContent(key);
            await httpClient.DeleteAsync(request, content);
        }

        public async Task ClearAsync(string tournamentUrl)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants/clear.json";

            KeyValuePair<string, string> key = new KeyValuePair<string, string>("api_key", apiKey);

            FormUrlEncodedContent content = new FormUrlEncodedContent(key);
            await httpClient.DeleteAsync(request, content);
        }

        public async Task RandomizeAsync(string tournamentUrl)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{tournamentUrl}/participants/randomize.json";

            KeyValuePair<string, string> key = new KeyValuePair<string, string>("api_key", apiKey);

            FormUrlEncodedContent content = new FormUrlEncodedContent(key);
            await httpClient.PostAsync(request, content);
        }
    }
}
