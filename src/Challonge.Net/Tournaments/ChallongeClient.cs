using Challonge.Matches;
using Challonge.Participants;
using Challonge.Tournaments;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// This class contains all the functions related to the tournaments api
        /// </summary>
        public class TournamentHandler
        {
            private readonly string _apiKey;
            private readonly HttpClient _httpClient;

            public TournamentHandler(string apiKey, HttpClient httpClient)
            {
                _apiKey = apiKey;
                _httpClient = httpClient;
            }

            private async Task<TournamentApiResult> TournamentApiCallAsync(string request, bool includeMatches, bool includeParticipants)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = _apiKey
                };

                if (includeMatches)
                    parameters["include_matches"] = "1";

                if (includeParticipants)
                    parameters["include_participants"] = "1";

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
                HttpResponseMessage response = await _httpClient.PostAsync(request, content);

                // raises the errors if there are any
                string responseString = await ErrorHandler.ParseResponseAsync(response);

                return await ParseTournamentApiResultAsync(responseString, includeMatches, includeParticipants);
            }

            private static async Task<TournamentApiResult> ParseTournamentApiResultAsync(string responseString, bool includeMatches, bool includeParticipants)
            {
                TournamentApiResult result = new TournamentApiResult();

                JsonElement rootElement = JsonDocument.Parse(responseString).RootElement;

                if (includeMatches)
                {
                    JsonElement matchesElement = rootElement.GetProperty("tournament").GetProperty("matches");

                    MemoryStream reader = new MemoryStream(Encoding.UTF8.GetBytes(matchesElement.GetRawText()));

                    MatchData[] matches = await JsonSerializer.DeserializeAsync<MatchData[]>(reader);
                    Match[] matchesResult = new Match[matches.Length];

                    for (int i = 0; i < matches.Length; i++)
                        matchesResult[i] = matches[i].Match;

                    result.Matches = matchesResult;
                }
                else
                    result.Matches = null;

                if (includeParticipants)
                {
                    JsonElement participantsElement = rootElement.GetProperty("tournament").GetProperty("participants");

                    MemoryStream reader = new MemoryStream(Encoding.UTF8.GetBytes(participantsElement.GetRawText()));
                    ParticipantData[] participants = await JsonSerializer.DeserializeAsync<ParticipantData[]>(reader);

                    Participant[] participantsResult = new Participant[participants.Length];

                    for (int i = 0; i < participants.Length; i++)
                        participantsResult[i] = participants[i].Participant;

                    result.Participants = participantsResult;
                }
                else
                    result.Participants = null;

                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
                TournamentData tournamentData = await JsonSerializer.DeserializeAsync<TournamentData>(stream);
                result.Tournament = tournamentData.Tournament;

                return result;
            }

            private Dictionary<string, string> PrepareParams(string name, string url, TournamentType type,
                string subdomain, string description, bool openSignup, bool holdThirdPlaceMatch,
                float ptsForMatchWin, float ptsForMatchTie, float ptsForGameWin, float ptsForGameTie,
                float ptsForBye, int? swissRounds, TournamentRankingStats rankedBy, bool acceptAttachments,
                bool hideForum, bool showRounds, bool isPrivate, bool notifyUsersWhenMatchesOpen,
                bool notifyUsersWhenTournamentsEnds, bool sequentialPairings, int? signupCap,
                DateTimeOffset? startAt, int? checkInDuration, TournamentGrandFinals grandFinalsModifier)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = _apiKey
                };
    
                if (name != null)
                    parameters["tournament[name]"] = name;

                if (url != null)
                    parameters["tournament[url]"] = url;

                parameters["tournament[tournament_type]"] = type switch
                {
                    TournamentType.SingleElimination => "single elimination",
                    TournamentType.DoubleElimination => "double elimination",
                    TournamentType.RoundRobin => "round robin",
                    TournamentType.Swiss => "swiss",
                    _ => ""
                };

                if (subdomain != null)
                    parameters["tournament[subdomain]"] = subdomain;

                if (description != null)
                    parameters["tournament[description]"] = description;

                parameters["tournament[open_signup]"] = openSignup.ToString().ToLower();

                parameters["tournament[hold_third_place_match]"] = holdThirdPlaceMatch.ToString().ToLower();

                parameters["tournament[pts_for_match_win]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForMatchWin);
                parameters["tournament[pts_for_match_tie]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForMatchTie);
                parameters["tournament[pts_for_game_win]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForGameWin);
                parameters["tournament[pts_for_game_tie]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForGameTie);
                parameters["tournament[pts_for_bye]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForBye);

                parameters["tournament[rr_pts_for_match_win]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForMatchWin);
                parameters["tournament[rr_pts_f, or_match_tie]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForMatchTie);
                parameters["tournament[rr_pts_for_game_tie]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForGameTie);
                parameters["tournament[rr_pts_fo r_game_win]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", ptsForGameWin);
                if (swissRounds != null)
                    parameters["swiss_rounds]"] = swissRounds.Value.ToString();

                parameters["tournament[ranked_by]"] = rankedBy switch
                {
                    // TODO check if the api accepts game wins percentage
                    TournamentRankingStats.MatchWins => "match wins",
                    TournamentRankingStats.GameWins => "game wins",
                    TournamentRankingStats.PointsScored => "points scored",
                    TournamentRankingStats.PointsDifference => "points difference",
                    TournamentRankingStats.Custom => "custom",
                    _ => parameters["tournament[ranked_by]"]
                };

                parameters["tournament[accept_attachments]"] = acceptAttachments.ToString().ToLower();

                parameters["tournament[hide_forum]"] = hideForum.ToString().ToLower();

                parameters["tournament[show_rounds]"] = showRounds.ToString().ToLower();

                parameters["tournament[private]"] = isPrivate.ToString().ToLower();

                parameters["tournament[notify_users_when_matches_open]"] = notifyUsersWhenMatchesOpen.ToString().ToLower();

                parameters["tournament[notify_users_when_the_tournament_ends]"] = notifyUsersWhenTournamentsEnds.ToString().ToLower();

                parameters["tournament[sequential_pairings]"] = sequentialPairings.ToString().ToLower();

                if (signupCap != null)
                    parameters["tournament[signup_cap]"] = signupCap.Value.ToString();

                if (startAt != null)
                    parameters["tournament[start_at]"] = startAt.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");

                if (checkInDuration != null)
                    parameters["tournament[check_in_duration]"] = checkInDuration.Value.ToString();

                parameters["tournament[grand_finals_modifier]"] = grandFinalsModifier switch
                {
                    TournamentGrandFinals.TwoChances => "",
                    TournamentGrandFinals.Skip => "skip",
                    TournamentGrandFinals.SingleMatch => "single match",
                    _ => parameters["tournament[grand_finals_modifier]"]
                };

                return parameters;
            }

            /// <summary>
            /// Retrieve a set of tournaments created with your account. 
            /// </summary>
            /// <param name="state">Either <see cref="TournamentState.Pending"/>, 
            /// <see cref="TournamentState.Underway"/> or <see cref="TournamentState.Completed"/>. Any other
            /// value will result in a query searching for all tournaments.</param>
            /// <param name="type">The type of the tournaments you want to retrieve</param>
            /// <param name="createdBefore"></param>
            /// <param name="createdAfter"></param>
            /// <param name="subdomain">A Challonge subdomain you've published tournaments to. 
            /// This is currently required to retrieve a list of your  organization-hosted tournaments.</param>
            /// <returns>The tournaments matching the given query</returns>
            public async Task<Tournament[]> GetTournamentsAsync(TournamentState? state = null, TournamentType? type = null,
            DateTimeOffset? createdBefore = null, DateTimeOffset? createdAfter = null, string subdomain = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments.json?api_key={_apiKey}";
                switch (state)
                {
                    case TournamentState.Pending:
                        request += "&state=pending";
                        break;
                    case TournamentState.Underway:
                        request += "&state=in_progress";
                        break;
                    case TournamentState.Completed:
                        request += "&state=ended";
                        break;
                }

                switch (type)
                {
                    case TournamentType.SingleElimination:
                        request += "&type=single_elimination";
                        break;
                    case TournamentType.DoubleElimination:
                        request += "&type=double_elimination";
                        break;
                    case TournamentType.RoundRobin:
                        request += "&type=round_robin";
                        break;
                    case TournamentType.Swiss:
                        request += "&type=swiss";
                        break;
                }

                if (createdAfter != null)
                    request += "&created_after=" + createdAfter.Value.ToString("yyyy-MM-dd");

                if (createdBefore != null)
                    request += "&created_before=" + createdBefore.Value.ToString("yyyy-MM-dd");

                if (subdomain != null)
                    request += "&subdomain=" + subdomain;

                TournamentData[] tournamentDatas = await GetAsync<TournamentData[]>(_httpClient, request);
                Tournament[] tournaments = new Tournament[tournamentDatas.Length];

                for (int i = 0; i < tournamentDatas.Length; i++)
                    tournaments[i] = tournamentDatas[i].Tournament;

                return tournaments;
            }

            /// <summary>
            /// Creates a new tournament
            /// </summary>
            /// <param name="builder">Tournament to create. Make sure its url is unique 
            /// (you can leave it blank and have challonge pick a random url for you)</param>
            /// <returns>The tournament that was created</returns>
            public async Task<Tournament> CreateTournamentAsync(TournamentBuilder builder)
            {
                return await CreateTournamentAsync(builder.Name, builder.Url, builder.TournamentType, builder.Subdomain,
                    builder.Description, builder.OpenSignup, builder.HoldThirdPlaceMatch, builder.PointsForMatchWin,
                    builder.PointsForMatchTie, builder.PointsForGameWin, builder.PointsForGameTie,
                    builder.PointsForBye, builder.SwissRounds, builder.RankedBy, builder.AcceptAttachments, 
                    builder.HideForum, builder.ShowRounds, builder.IsPrivate, builder.NotifyUsersWhenMatchesOpen,
                    builder.NotifyUsersWhenTournamentEnds, builder.SequentialPairings, builder.SignupCap,
                    builder.StartAt, builder.CheckInDuration);
            }

            /// <summary>
            /// Creates a new tournament
            /// </summary>
            /// <param name="url">Url of your tournament (letters, numbers and underscores only). 
            /// Make sure it is unique (you can leave it blank and have challonge pick a random url for you)</param>
            /// <param name="name">Name of the tournament</param>
            /// <param name="type">Type of tournament</param>
            /// <param name="subdomain">Subdomain to assign the tournament to (requires write access to
            /// the subdomain)</param>
            /// <param name="description">Description/instructions to be displayed above the bracket</param>
            /// <param name="openSignup">Have Challonge host a sign-up page (otherwise, you manually add all participants)</param>
            /// <param name="holdThirdPlaceMatch">Single Elimination only. Include a match between semifinal losers?</param>
            /// <param name="ptsForMatchWin">Swiss only: number of points gained for a match win</param>
            /// <param name="ptsForMatchTie">Swiss only: number of points gained for a match tie</param>
            /// <param name="ptsForGameWin">Swiss only: number of points gained for a game win</param>
            /// <param name="ptsForGameTie">Swiss only: number of points gained for a game tie</param>
            /// <param name="ptsForBye">Swiss only: number of points gained for a bye</param>
            /// <param name="swissRounds">Number of rounds</param>
            /// <param name="rankedBy">How the participants are ranked</param>
            /// <param name="acceptAttachments">Allow match attachment uploads</param>
            /// <param name="hideForum">Hide the forum tab on your Challonge page</param>
            /// <param name="showRounds">Whether or not to label each round above the bracket</param>
            /// <param name="isPrivate">Hide this tournament from the public browsable index and your profile</param>
            /// <param name="notifyUsersWhenMatchesOpen">Whether or not to email registered Challonge 
            /// participants when matches open up for them</param>
            /// <param name="notifyUsersWhenTournamentsEnds">Whether or not to email registered Challonge
            /// participants when this tournament ends</param>
            /// <param name="sequentialPairings">When enabled, instead of traditional seeding rules, make 
            /// pairings by going straight down the list of participants. First round matches are filled in top 
            /// to bottom, then qualifying matches (if applicable).</param>
            /// <param name="signupCap">Maximum number of participants in the bracket. Once the cap is reached, 
            /// new participants will be put in waiting list, and <see cref="Participant.OnWaitingList"/> will 
            /// be set to <see langword="true"/></param>
            /// <param name="startAt"></param>
            /// <param name="checkInDuration">Length of the participant check-in window in minutes.</param>
            /// <param name="grandFinalsModifier">Double elimination only: how the grand finals will be played</param>
            /// <returns>The tournament that was created</returns>
            public async Task<Tournament> CreateTournamentAsync(string name, string url = null, 
                TournamentType type = TournamentType.SingleElimination, string subdomain = null, string description = null, 
                bool openSignup = false, bool holdThirdPlaceMatch = false, float ptsForMatchWin = 1.0f, 
                float ptsForMatchTie = 0.5f, float ptsForGameWin = 0f, float ptsForGameTie = 0f,
                float ptsForBye = 1.0f, int? swissRounds = null, TournamentRankingStats rankedBy = TournamentRankingStats.MatchWins,
                bool acceptAttachments = false, bool hideForum = false, bool showRounds = false, bool isPrivate = false,
                bool notifyUsersWhenMatchesOpen = false, bool notifyUsersWhenTournamentsEnds = false, 
                bool sequentialPairings = false, int? signupCap = null, DateTimeOffset? startAt = null, 
                int? checkInDuration = null, TournamentGrandFinals grandFinalsModifier = TournamentGrandFinals.TwoChances)
            {
                string request = "https://api.challonge.com/v1/tournaments.json";

                Dictionary<string, string> parameters = PrepareParams(name, url, type, subdomain, description,
                    openSignup, holdThirdPlaceMatch, ptsForMatchWin, ptsForMatchTie, ptsForGameWin, ptsForGameTie,
                    ptsForBye, swissRounds, rankedBy,
                    acceptAttachments, hideForum, showRounds, isPrivate, notifyUsersWhenMatchesOpen,
                    notifyUsersWhenTournamentsEnds, sequentialPairings, signupCap, startAt, checkInDuration,
                    grandFinalsModifier);

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

                TournamentData tournamentData = await PostAsync<TournamentData>(_httpClient, request, content);
                return tournamentData.Tournament;
            }

            /// <summary>
            /// Retrieve a single tournament record created with your account.
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be "subdomain-tournament_url"
            /// (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> GetTournamentAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}.json?api_key={_apiKey}";

                if (includeMatches)
                    request += "&include_matches=1";

                if (includeParticipants)
                    request += "&include_participants=1";

                HttpResponseMessage response = await _httpClient.GetAsync(request);
                string responseString = await ErrorHandler.ParseResponseAsync(response);

                return await ParseTournamentApiResultAsync(responseString, includeMatches, includeParticipants);
            }

            /// <summary>
            /// Updates an existing tournament
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be "subdomain-tournament_url"
            /// (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="builder">The builder containing the properties to be updated</param>
            /// <returns>The updated tournament</returns>
            public async Task<Tournament> UpdateTournamentAsync(string tournament, TournamentBuilder builder)
            {
                return await UpdateTournamentAsync(tournament, builder.Name, builder.Url, builder.TournamentType,
                    builder.Subdomain, builder.Description, builder.OpenSignup, builder.HoldThirdPlaceMatch,
                    builder.PointsForMatchWin, builder.PointsForMatchTie, builder.PointsForGameWin,
                    builder.PointsForGameTie, builder.PointsForBye, builder.SwissRounds, builder.RankedBy,
                    builder.AcceptAttachments, builder.HideForum, builder.ShowRounds, builder.IsPrivate,
                    builder.NotifyUsersWhenMatchesOpen, builder.NotifyUsersWhenTournamentEnds, builder.SequentialPairings, 
                    builder.SignupCap, builder.StartAt, builder.CheckInDuration, builder.GrandFinalsModifier);
            }
            
            /// <summary>
            /// Updates an existing tournament
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for challonge.com/single_elim). If assigned to a subdomain, URL format must be "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney) </param>
            /// <param name="url">Url of your tournament (letters, numbers and underscores only). 
            /// Make sure it is unique (you can leave it blank and have challonge pick a random url for you)</param>
            /// <param name="name">Name of the tournament</param>
            /// <param name="type">Type of tournament</param>
            /// <param name="subdomain">Subdomain to assign the tournament to (requires write access to
            /// the subdomain)</param>
            /// <param name="description">Description/instructions to be displayed above the bracket</param>
            /// <param name="openSignup">Have Challonge host a sign-up page (otherwise, you manually add all participants)</param>
            /// <param name="holdThirdPlaceMatch">Single Elimination only. Include a match between semifinal losers?</param>
            /// <param name="ptsForMatchWin">Swiss only: number of points gained for a match win</param>
            /// <param name="ptsForMatchTie">Swiss only: number of points gained for a match tie</param>
            /// <param name="ptsForGameWin">Swiss only: number of points gained for a game win</param>
            /// <param name="ptsForGameTie">Swiss only: number of points gained for a game tie</param>
            /// <param name="ptsForBye">Swiss only: number of points gained for a bye</param>
            /// <param name="swissRounds">Number of rounds</param>
            /// <param name="rankedBy">How the participants are ranked</param>
            /// <param name="acceptAttachments">Allow match attachment uploads</param>
            /// <param name="hideForum">Hide the forum tab on your Challonge page</param>
            /// <param name="showRounds">Whether or not to label each round above the bracket</param>
            /// <param name="isPrivate">Hide this tournament from the public browsable index and your profile</param>
            /// <param name="notifyUsersWhenMatchesOpen">Whether or not to email registered Challonge 
            /// participants when matches open up for them</param>
            /// <param name="notifyUsersWhenTournamentsEnds">Whether or not to email registered Challonge
            /// participants when this tournament ends</param>
            /// <param name="sequentialPairings">When enabled, instead of traditional seeding rules, make 
            /// pairings by going straight down the list of participants. First round matches are filled in top 
            /// to bottom, then qualifying matches (if applicable).</param>
            /// <param name="signupCap">Maximum number of participants in the bracket. Once the cap is reached, 
            /// new participants will be put in waiting list, and <see cref="Participant.OnWaitingList"/> will 
            /// be set to <see langword="true"/></param>
            /// <param name="startAt"></param>
            /// <param name="checkInDuration">Length of the participant check-in window in minutes.</param>
            /// <param name="grandFinalsModifier">Double elimination only: how the grand finals will be played</param>
            /// <returns>The updated tournament</returns>
            public async Task<Tournament> UpdateTournamentAsync(string tournament, string name = null,
                string url = null, TournamentType type = TournamentType.SingleElimination, string subdomain = null,
                string description = null, bool openSignup = false, bool holdThirdPlaceMatch = false, 
                float ptsForMatchWin = 1.0f, float ptsForMatchTie = 0.5f, float ptsForGameWin = 0f, 
                float ptsForGameTie = 0f, float ptsForBye = 1.0f, int? swissRounds = null, 
                TournamentRankingStats rankedBy = TournamentRankingStats.MatchWins, bool acceptAttachments = false, 
                bool hideForum = false, bool showRounds = false, bool isPrivate = false, bool notifyUsersWhenMatchesOpen = false, 
                bool notifyUsersWhenTournamentsEnds = false, bool sequentialPairings = false, int? signupCap = null,
                DateTimeOffset? startAt = null, int? checkInDuration = null, 
                TournamentGrandFinals grandFinalsModifier = TournamentGrandFinals.TwoChances)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}.json";

                Dictionary<string, string> parameters = PrepareParams(name, url, type, subdomain, description,
                    openSignup, holdThirdPlaceMatch, ptsForMatchWin, ptsForMatchTie, ptsForGameWin, ptsForGameTie,
                    ptsForBye, swissRounds, rankedBy, acceptAttachments, hideForum, showRounds, isPrivate, 
                    notifyUsersWhenMatchesOpen, notifyUsersWhenTournamentsEnds, sequentialPairings, signupCap, startAt, 
                    checkInDuration, grandFinalsModifier);

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

                TournamentData tournamentData = await PutAsync<TournamentData>(_httpClient, request, content);
                return tournamentData.Tournament;
            }

            /// <summary>
            /// Deletes a tournament along with all its associated records. There is no undo, so use with care!
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            public async Task DeleteTournamentAsync(string tournament)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}.json?api_key={_apiKey}";

                await DeleteAsync(_httpClient, request);
            }

            /// <summary>
            /// This should be invoked after a tournament's check-in window closes before the tournament is started.
            /// <list type="number">
            /// <item>Marks participants who have not checked in as inactive.</item>
            /// <item>Moves inactive participants to bottom seeds (ordered by original seed).</item>
            /// <item>Transitions the tournament state from <see cref="TournamentState.CheckingIn"/> 
            /// to <see cref="TournamentState.CheckedIn"/></item>
            /// </list>
            /// </summary>
            /// <remarks>Checked in participants on the waiting list will be promoted if slots become available.</remarks>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> ProcessCheckInsAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/process_check_ins.json";

                return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
            }

            /// <summary>
            /// Aborts the check-in process: makes all participants active and clears their 
            /// <see cref="Participant.CheckedInAt"/> times and sets the tournament state from 
            /// <see cref="TournamentState.CheckedIn"/> or <see cref="TournamentState.CheckingIn"/> to
            /// <see cref="TournamentState.Pending"/>
            /// </summary>
            /// <remarks>
            /// When your tournament is in a <see cref="TournamentState.CheckingIn"/> or
            /// <see cref="TournamentState.CheckedIn"/> state, there's no way to edit the tournament's start time
            /// (<see cref="Tournament.StartAt"/>) or <see cref="Tournament.CheckInDuration"/>. You must first abort
            /// check-in, then you may edit those attributes.
            /// </remarks>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> AbortCheckInAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/abort_check_in.json";

                return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
            }

            /// <summary>
            /// Start a tournament, opening up first round matches for score reporting. The tournament must 
            /// have at least 2 participants
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> StartTournamentAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/start.json";

                return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
            }

            /// <summary>
            /// Finalize a tournament that has had all match scores submitted, rendering its results permanent. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> FinalizeTournamentAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/finalize.json";

                return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
            }

            /// <summary>
            /// Resets a tournament, clearing all of its scores and attachments. You can then add/remove/edit 
            /// participants before starting the tournament again. 
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> ResetTournamentAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/reset.json";

                return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
            }

            /// <summary>
            /// Sets the state of the tournament to start accepting predictions. Your <see 
            /// cref="Tournament.PredictionMethod"/> attribute must be set to 
            /// <see cref="TournamentPredictionMethod.Exponential"/> or 
            /// <see cref="TournamentPredictionMethod.Linear"/> to use this option
            /// </summary>
            /// <remarks>
            /// Once open for predictions, match records will be persisted, so participant additions and 
            /// removals will no longer be permitted. 
            /// </remarks>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// "subdomain-tournament_url" (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="includeMatches"></param>
            /// <param name="includeParticipants"></param>
            /// <returns></returns>
            public async Task<TournamentApiResult> OpenTournamentForPredictionsAsync(string tournament, bool includeMatches = false, bool includeParticipants = false)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/open_for_predictions.json";

                return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
            }
        }
    }
}
