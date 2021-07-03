using Challonge.Attachments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Challonge
{
    public partial class ChallongeClient
    {
        /// <summary>
        /// This class contains all the functions related to the attachments api
        /// </summary>
        public class AttachmentsHandler
        {
            private readonly string _apiKey;
            private readonly HttpClient _httpClient;

            public AttachmentsHandler(string apiKey, HttpClient httpClient)
            {
                _apiKey = apiKey;
                _httpClient = httpClient;
            }

            /// <summary>
            /// Retrieve a match's attachments
            /// </summary>
            /// <returns></returns>
            public async Task<Attachment[]> GetAttachmentsAsync(string tournament, int matchId)
            {
                string request = $" https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments.json?api_key={_apiKey}";

                AttachmentData[] attachmentDatas = await GetAsync<AttachmentData[]>(_httpClient, request);
                Attachment[] result = new Attachment[attachmentDatas.Length];

                for (int i = 0; i < attachmentDatas.Length; i++)
                    result[i] = attachmentDatas[i].Attachment;

                return result;
            }

            /// <summary>
            /// Creates an attachment
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="url">A web url</param>
            /// <param name="description">Text to describe the URL attachment, or this can simply be 
            /// standalone text</param>
            /// <returns>The created attachment</returns>
            /// <remarks>
            /// <list type="bullet">The associated tournament's <see cref="Tournaments.Tournament.AcceptAttachments"/> 
            /// attribute must be <see langword="true"/> for this action to succeed. </list>
            /// <list type="bullet">At least one of the two optional parameters must be provided.</list>
            /// </remarks>
            public async Task<Attachment> CreateAttachmentAsync(string tournament, int matchId,
                /*FileStream file = null,*/ string url = null, string description = null)
            {
                if (/*file == null &&*/ url == null && description == null)
                    throw new ArgumentNullException("At least 1 of the 2 optional parameters must be provided.");

                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments.json";

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = _apiKey
                };

                if (url != null)
                    parameters["match_attachment[url]"] = url;

                if (description != null)
                    parameters["match_attachment[description]"] = description;

                //MultipartFormDataContent form = new MultipartFormDataContent();
                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
                //form.Add(content);

                //if (file != null)
                //{
                //    byte[] bytes = new byte[file.Length];
                //    await file.ReadAsync(bytes, 0, (int)file.Length);
                //    file.Close();
                //    ByteArrayContent byteArrayContent = new ByteArrayContent(bytes);
                //    form.Add(byteArrayContent, "match_attachment[asset]");

                //    //StreamContent stream = new StreamContent(file);
                //    //form.Add(stream, "match_attachment[asset]");
                //}

                AttachmentData attachmentData = await PostAsync<AttachmentData>(_httpClient, request, content);
                return attachmentData.Attachment;
            }

            /// <summary>
            /// Retrieve a single match attachment record
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="attachmentId">The attachment's unique id</param>
            /// <returns></returns>
            public async Task<Attachment> GetAttachmentAsync(string tournament, int matchId, int attachmentId)
            {
                string request = $" https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments/{attachmentId}.?api_key={_apiKey}";

                AttachmentData attachmentData = await GetAsync<AttachmentData>(_httpClient, request);
                return attachmentData.Attachment;
            }

            /// <summary>
            /// Update the attributes of a match attachment
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="attachmentId">The tournament's unique id</param>
            /// <param name="url">A web URL</param>
            /// <param name="description">Text to describe the file or URL attachment, or this can simply be 
            /// standalone text</param>
            /// <returns>The updated attachment</returns>
            /// <remarks>At least one of the two optional parameters must be provided</remarks>
            public async Task<Attachment> UpdateAttachmentAsync(string tournament, int matchId, int attachmentId,
                string url = null, string description = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments/{attachmentId}.json";

                if (url == null && description == null)
                    throw new ArgumentNullException("At least 1 of the 3 optional parameters must be provided.");

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = _apiKey
                };

                if (url != null)
                    parameters["match_attachment[url]"] = url;

                if (description != null)
                    parameters["match_attachment[description]"] = description;

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

                AttachmentData attachmentData = await PutAsync<AttachmentData>(_httpClient, request, content);
                return attachmentData.Attachment;
            }

            /// <summary>
            /// Deletes a match attachment
            /// </summary>
            /// <param name="tournament">Tournament ID (e.g. 10230) or URL (e.g. 'single_elim' for 
            /// challonge.com/single_elim). If assigned to a subdomain, URL format must be 
            /// subdomain-tournament_url (e.g. 'test-mytourney' for test.challonge.com/mytourney)</param>
            /// <param name="matchId">The match's unique id</param>
            /// <param name="attachmentId">The attachment's unique id</param>
            /// <returns></returns>
            public async Task DeleteAttachmentAsync(string tournament, int matchId, int attachmentId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments/{attachmentId}.json?api_key={_apiKey}";

                await DeleteAsync(_httpClient, request);
            }
        }
    }
}
