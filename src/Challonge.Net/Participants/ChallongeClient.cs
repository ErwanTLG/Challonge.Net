using Challonge.Matches;
using Challonge.Participants;
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
        /// <summary>
        /// This class contains all the functions related to the participants api
        /// </summary>
        public class ParticipantsHandler
        {
            private readonly string apiKey;
            private readonly HttpClient httpClient;

            public ParticipantsHandler(string apiKey, HttpClient httpClient)
            {
                this.apiKey = apiKey;
                this.httpClient = httpClient;
            }

            /// <summary>
            /// Retrieve a tournament's participant list. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <returns>The list of participants of the given tournament</returns>
            public async Task<Participant[]> GetParticipantsAsync(string tournament)
            {
                string request = $" https://api.challonge.com/v1/tournaments/{tournament}/participants.json?api_key={apiKey}";

                ParticipantData[] participantDatas = await GetAsync<ParticipantData[]>(httpClient, request);
                Participant[] participants = new Participant[participantDatas.Length];

                for (int i = 0; i < participantDatas.Length; i++)
                    participants[i] = participantDatas[i].Participant;

                return participants;
            }

            /// <summary>
            /// Add a participant to a tournament (up until it is started)
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="name">The name displayed in the bracket/schedule - not required if email 
            /// or challongeUsername is provided. Must be unique per tournament. </param>
            /// <param name="challongeUsername">Provide this if the participant has a Challonge account. 
            /// He or she will be invited to the tournament. </param>
            /// <param name="email">Providing this will first search for a matching Challonge account. If one 
            /// is found, this will have the same effect as the "challonge_username" attribute. If one is not
            /// found, the "new-user-email" attribute will be set, and the user will be invited via email to
            /// create an account. </param>
            /// <param name="seed">The participant's new seed. Must be between 1 and the current number of
            /// participants (including the new record). Overwriting an existing seed will automatically bump
            /// other participants as you would expect. </param>
            /// <param name="misc"> Max: 255 characters. Multi-purpose field that is only visible via the API 
            /// and handy for site integration (e.g. key to your users table) </param>
            /// <returns>The created participant</returns>
            public async Task<Participant> CreateParticipantAsync(string tournament, string name = null,
                string challongeUsername = null, string email = null, int? seed = null, string misc = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants.json";

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

                ParticipantData participantData = await PostAsync<ParticipantData>(httpClient, request, content);
                return participantData.Participant;
            }

            /// <summary>
            /// Add a participant to a tournament (up until it is started)
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="participant">Participant to create</param>
            /// <returns>The created participant</returns>
            /// <remarks>The returned participant will differ from the one you passed as an argument,
            /// because the api will set some properties that you have not write access to.</remarks>
            public async Task<Participant> CreateParticipantAsync(string tournament, Participant participant)
            {
                return await CreateParticipantAsync(tournament, participant.Name, participant.ChallongeUsername,
                    participant.InviteEmail, participant.Seed, participant.Misc);
            }

            public async Task<Participant[]> CreateParticipantsAsync(string tournament, string[] names = null,
                string[] challongeUsernames = null, int[] seeds = null, string[] miscs = null)
            {
                string request = $" https://api.challonge.com/v1/tournaments/{tournament}/participants/bulk_add.json";

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("api_key", apiKey)
                };

                for (int i = 0; i < names.Length; i++)
                {
                    if (names != null)
                        parameters.Add(new KeyValuePair<string, string>("participants[][name]", names[i]));

                    if (challongeUsernames != null)
                        parameters.Add(new KeyValuePair<string, string>("participants[][invite_name_or_email]",
                            challongeUsernames[i]));

                    if (seeds != null)
                        parameters.Add(new KeyValuePair<string, string>("participants[][seed]", seeds[i].ToString()));

                    if (miscs != null)
                        parameters.Add(new KeyValuePair<string, string>("participants[][misc]", miscs[i]));
                }

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
                ParticipantData[] participantDatas = await PostAsync<ParticipantData[]>(httpClient, request, content);

                Participant[] participants = new Participant[participantDatas.Length];

                for (int i = 0; i < participantDatas.Length; i++)
                    participants[i] = participantDatas[i].Participant;

                return participants;
            }

            /// <summary>
            /// Retrieve a single participant record for a tournament
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="participantId">The participant's unique ID </param>
            /// <param name="includeMatches"></param>
            /// <returns>A result containing the participant and if requested its match records</returns>
            public async Task<ParticipantApiResult> GetParticipantAsync(string tournament, int participantId, bool includeMatches = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants/{participantId}.json?api_key={apiKey}";

                if (includeMatches)
                    request += "&include_matches=1";

                HttpResponseMessage response = await httpClient.GetAsync(request);
                string responseString = await ErrorHandler.ParseResponseAsync(response);

                ParticipantApiResult result = new ParticipantApiResult();

                if (includeMatches)
                {
                    JsonElement matchesElement = JsonDocument.Parse(responseString).RootElement.GetProperty("participant").GetProperty("matches");

                    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(matchesElement.GetRawText()));

                    MatchData[] matches = await JsonSerializer.DeserializeAsync<MatchData[]>(stream);
                    Match[] matchesResult = new Match[matches.Length];

                    for (int i = 0; i < matches.Length; i++)
                        matchesResult[i] = matches[i].Match;

                    result.Matches = matchesResult;
                }
                else
                    result.Matches = null;

                MemoryStream reader = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
                ParticipantData participantData = await JsonSerializer.DeserializeAsync<ParticipantData>(reader);
                result.Participant = participantData.Participant;

                return result;
            }

            /// <summary>
            /// Update the attributes of a tournament participant
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="name">The name displayed in the bracket/schedule - not required if email 
            /// or challongeUsername is provided. Must be unique per tournament. </param>
            /// <param name="challongeUsername">Provide this if the participant has a Challonge account. 
            /// He or she will be invited to the tournament. </param>
            /// <param name="email">Providing this will first search for a matching Challonge account. If one 
            /// is found, this will have the same effect as the "challonge_username" attribute. If one is not
            /// found, the "new-user-email" attribute will be set, and the user will be invited via email to
            /// create an account. </param>
            /// <param name="seed">The participant's new seed. Must be between 1 and the current number of
            /// participants (including the new record). Overwriting an existing seed will automatically bump
            /// other participants as you would expect. </param>
            /// <param name="misc"> Max: 255 characters. Multi-purpose field that is only visible via the API 
            /// and handy for site integration (e.g. key to your users table) </param>
            /// <returns>The updated participant</returns>
            public async Task<Participant> UpdateParticipantAsync(string tournament, int participantId, string name = null,
                string challongeUsername = null, string email = null, int? seed = null, string misc = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants{participantId}.json";

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

                ParticipantData participantData = await PutAsync<ParticipantData>(httpClient, request, content);
                return participantData.Participant;
            }

            /// <summary>
            /// Update the attributes of a tournament participant
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="participant">Updated participant</param>
            /// <returns>The updated participant</returns>
            /// <remarks>The returned participant will differ from the one you passed as an argument.
            /// (the value UpdatedAt will have changed)</remarks>
            public async Task<Participant> UpdateParticipantAsync(string tournament, Participant participant)
            {
                return await UpdateParticipantAsync(tournament, participant.Id, participant.Name,
                    participant.ChallongeUsername, participant.InviteEmail, participant.Seed, participant.Misc);
            }

            /// <summary>
            /// Checks a participant in, setting <see cref="Participant.CheckedInAt"/> to the current time. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="participantId">The participant's unique id</param>
            public async Task CheckInAsync(string tournament, int participantId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants/{participantId}/check_in.json";

                await ApiCallAsync(httpClient, apiKey, request);
            }

            /// <summary>
            /// Marks a participant as having not checked in, setting <see cref="Participant.CheckedInAt"/> to
            /// <see langword="null"/>. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="participantId">The participant's unique id</param>
            public async Task UndoCheckInAsync(string tournament, int participantId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants/{participantId}/undo_check_in.json";

                await ApiCallAsync(httpClient, apiKey, request);
            }

            /// <summary>
            /// If the tournament has not started, delete a participant, automatically filling in the abandoned
            /// seed number. If tournament is underway, mark a participant inactive, automatically 
            /// forfeiting his/her remaining matches.
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="participantId">The participant's unique id</param>
            /// <returns></returns>
            public async Task DeleteParticipantAsync(string tournament, int participantId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants/{participantId}.json?api_key={apiKey}";

                await DeleteAsync(httpClient, request);
            }

            /// <summary>
            /// Deletes all participants in a tournament. (Only allowed if tournament hasn't started yet) 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <returns></returns>
            public async Task ClearAsync(string tournament)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants/clear.json?api_key={apiKey}";

                await DeleteAsync(httpClient, request);
            }

            /// <summary>
            /// Randomize seeds among participants. Only applicable before a tournament has started.
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <returns></returns>
            public async Task RandomizeAsync(string tournament)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/participants/randomize.json";

                await ApiCallAsync(httpClient, apiKey, request);
            }
        }
    }
}
