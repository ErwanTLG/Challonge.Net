using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Challonge.Matches;
using Challonge.Participants;
using Challonge.Tournaments;

namespace Challonge
{
    public partial class ChallongeClient
    {
        private async Task<TournamentApiResult> TournamentApiCallAsync(string request, bool includeMatches, bool includeParticipants)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey
            };

            if (includeMatches)
                parameters["include_matches"] = "1";

            if (includeParticipants)
                parameters["include_participants"] = "1";

            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PostAsync(request, content);

            // raises the errors if there are any
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            return ParseTournamentApiResult(responseString, includeMatches, includeParticipants);
        }

        // TODO make async
        private static TournamentApiResult ParseTournamentApiResult(string responseString, bool includeMatches, bool includeParticipants)
        {
            TournamentApiResult result = new TournamentApiResult();

            if (includeMatches)
            {
                JsonElement matchesElement = JsonDocument.Parse(responseString).RootElement.GetProperty("tournament").GetProperty("matches");
                MatchData[] matches = JsonSerializer.Deserialize<MatchData[]>(matchesElement.ToString());
                Match[] matchesResult = new Match[matches.Length];

                for (int i = 0; i < matches.Length; i++)
                    matchesResult[i] = matches[i].Match;

                result.Matches = matchesResult;
            }
            else
                result.Matches = null;

            if (includeParticipants)
            {
                JsonElement participantsElement = JsonDocument.Parse(responseString).RootElement.GetProperty("tournament").GetProperty("participants");
                ParticipantData[] participants = JsonSerializer.Deserialize<ParticipantData[]>(participantsElement.ToString());
                Participant[] participantsResult = new Participant[participants.Length];

                for (int i = 0; i < participants.Length; i++)
                    participantsResult[i] = participants[i].Participant;

                result.Participants = participantsResult;
            }
            else
                result.Participants = null;

            TournamentData tournamentData = JsonSerializer.Deserialize<TournamentData>(responseString);
            Tournament tournament = tournamentData.Tournament;

            result.Tournament = tournament;

            return result;
        }

        public async Task<Tournament[]> GetTournamentsAsync(TournamentState? state = null, TournamentType? type = null,
            DateTimeOffset? createdBefore = null, DateTimeOffset? createdAfter = null, string subdomain = null)
        {
            string request = $"https://api.challonge.com/v1/tournaments.json?api_key={apiKey}";
            switch (state)
            {
                case TournamentState.Pending:
                    request += "&state=pending";
                    break;
                case TournamentState.InProgress:
                    request += "&state=in_progress";
                    break;
                case TournamentState.Ended:
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

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            TournamentData[] tournamentDatas = JsonSerializer.Deserialize<TournamentData[]>(responseString);
            Tournament[] tournaments = new Tournament[tournamentDatas.Length];

            for (int i = 0; i < tournamentDatas.Length; i++)
                tournaments[i] = tournamentDatas[i].Tournament;

            return tournaments;
        }

        public async Task<Tournament> CreateTournamentAsync(Tournament tournament)
        {
            return await CreateTournamentAsync(tournament.Name, tournament.Url, tournament.TournamentType, tournament.Subdomain,
                tournament.Description, tournament.OpenSignup, tournament.HoldThirdPlaceMatch, tournament.PointsForMatchWin,
                tournament.PointsForMatchTie, tournament.PointsForGameWin, tournament.PointsForGameTie,
                tournament.PointsForBye, tournament.SwissRounds, tournament.RankedBy, tournament.RoundRobinPointsForMatchWin,
                tournament.RoundRobinPointsForMatchTie, tournament.RoundRobinPointsForGameWin, tournament.RoundRobinPointsForGameTie,
                tournament.AcceptAttachments, tournament.HideForum, tournament.ShowRounds, tournament.IsPrivate,
                tournament.NotifyUsersWhenMatchesOpen, tournament.NotifyUsersWhenTournamentEnds, tournament.SequentialPairing, tournament.SignupCap, tournament.StartAt, tournament.CheckInDuration);
        }

        public async Task<Tournament> CreateTournamentAsync(string name, string url, TournamentType type = TournamentType.SingleElimination,
            string subdomain = null, string description = null, bool openSignup = false, bool holdThirdPlaceMatch = false,
            float ptsForMatchWin = 1.0f, float ptsForMatchTie = 0.5f, float ptsForGameWin = 0f, float ptsForGameTie = 0f,
            float ptsForBye = 1.0f, int? swissRounds = null, string rankedBy = null/*RANKED BY HERE*/, float rrPtsForMatchWin = 1.0f,
            float rrPtsForMatchTie = 0.5f, float rrPtsForGameWin = 0f, float rrPtsForGameTie = 0f, bool acceptAttachments = false,
            bool hideForum = false, bool showRounds = false, bool isPrivate = false, bool notifyUsersWhenMatchesOpen = false,
            bool notifyUsersWhenTournamentsEnds = false, bool sequentialPairings = false, int? signupCap = null,
            DateTimeOffset? startAt = null, int? checkInDuration = null /*GRAND FINALS MODIFIER*/)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                ["api_key"] = apiKey,
                ["tournament[name]"] = name ?? throw new ArgumentNullException("name"),
                ["tournament[url]"] = url ?? throw new ArgumentNullException("url")
            };

            switch (type)
            {
                case TournamentType.SingleElimination:
                    parameters["tournament[tournament_type]"] = "single elimination";
                    break;
                case TournamentType.DoubleElimination:
                    parameters["tournament[tournament_type]"] = "double elimination";
                    break;
                case TournamentType.RoundRobin:
                    parameters["tournament[tournament_type]"] = "round robin";
                    break;
                case TournamentType.Swiss:
                    parameters["tournament[tournament_type]"] = "swiss";
                    break;
                default:
                    parameters["tournament[tournament_type]"] = "";
                    break;
            }

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

            if (swissRounds != null)
                parameters["swiss_rounds]"] = swissRounds.Value.ToString();


            // ranked by here
            parameters["tournament[ranked_by]"] = "match wins";

            parameters["tournament[rr_pts_for_match_win]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", rrPtsForMatchWin);
            parameters["tournament[rr_pts_f, or_match_tie]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", rrPtsForMatchTie);
            parameters["tournament[rr_pts_for_game_tie]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", rrPtsForGameTie);
            parameters["tournament[rr_pts_fo r_game_win]"] = string.Format(CultureInfo.InvariantCulture, "{0:G}", rrPtsForGameWin);

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
                parameters["tournament[start_at]"] = startAt.Value.ToString("");    //TODO fix format here

            if (checkInDuration != null)
                parameters["tournament[check_in_duration]"] = checkInDuration.Value.ToString();

            // grand final modifier
            parameters["tournament[grand_finals_modifier]"] = "";


            // request
            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await httpClient.PostAsync("https://api.challonge.com/v1/tournaments.json", content);

            string responseString = await ErrorHandler.ParseResponseAsync(response);
            TournamentData tournamentData = JsonSerializer.Deserialize<TournamentData>(responseString); //TODO make async

            return tournamentData.Tournament;
        }

        public async Task<TournamentApiResult> GetTournamentAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}.json?api_key={apiKey}";

            if (includeMatches)
                request += "&include_matches=1";

            if (includeParticipants)
                request += "&include_participants=1";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            return ParseTournamentApiResult(responseString, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> GetTournamentAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}.json?api_key={apiKey}";

            if (includeMatches)
                request += "&include_matches=1";

            if (includeParticipants)
                request += "&include_participants=1";

            HttpResponseMessage response = await httpClient.GetAsync(request);
            string responseString = await ErrorHandler.ParseResponseAsync(response);

            return ParseTournamentApiResult(responseString, includeMatches, includeParticipants);
        }

        public async Task<Tournament> DeleteTournamentAsync(string url)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}.json?api_key={apiKey}";
            HttpResponseMessage response = await httpClient.DeleteAsync(request);

            string responseString = await ErrorHandler.ParseResponseAsync(response);

            TournamentData tournamentData = JsonSerializer.Deserialize<TournamentData>(responseString);
            return tournamentData.Tournament;
        }

        public async Task<Tournament> DeleteTournamentAsync(int id)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}.json?api_key={apiKey}";
            HttpResponseMessage response = await httpClient.DeleteAsync(request);

            string responseString = await ErrorHandler.ParseResponseAsync(response); 
            
            TournamentData tournamentData = JsonSerializer.Deserialize<TournamentData>(responseString);
            return tournamentData.Tournament;
        }

        public async Task<TournamentApiResult> ProcessCheckInsAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}/process_check_ins.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> ProcessCheckInsAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}/process_check_ins.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> AbortCheckInAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}/abort_check_in.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> AbortCheckInAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}/abort_check_in.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> StartTournamentAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}/start.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> StartTournamentAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}/start.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> FinalizeTournamentAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}/finalize.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> FinalizeTournamentAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}/finalize.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> ResetTournamentAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}/reset.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> ResetTournamentAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}/reset.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> OpenTournamentForPredictionsAsync(string url, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{url}/open_for_predictions.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }

        public async Task<TournamentApiResult> OpenTournamentForPredictionsAsync(int id, bool includeMatches = false, bool includeParticipants = false)
        {
            string request = $"https://api.challonge.com/v1/tournaments/{id}/open_for_predictions.json";

            return await TournamentApiCallAsync(request, includeMatches, includeParticipants);
        }
    }
}
