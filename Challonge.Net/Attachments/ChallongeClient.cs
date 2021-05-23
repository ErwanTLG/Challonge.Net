using Challonge.Attachments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Challonge
{
    public partial class ChallongeClient
    {
        public class AttachmentsHandler
        {
            private readonly string apiKey;
            private readonly HttpClient httpClient;

            public AttachmentsHandler(string apiKey, HttpClient httpClient)
            {
                this.apiKey = apiKey;
                this.httpClient = httpClient;
            }

            /// <summary>
            /// Retrieve a match's attachments.
            /// </summary>
            /// <returns></returns>
            public async Task<Attachment[]> GetAttachmentsAsync(string tournament, int matchId)
            {
                string request = $" https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments.json?api_key={apiKey}";

                AttachmentData[] attachmentDatas = await GetAsync<AttachmentData[]>(httpClient, request);
                Attachment[] result = new Attachment[attachmentDatas.Length];

                for (int i = 0; i < attachmentDatas.Length; i++)
                    result[i] = attachmentDatas[i].Attachment;

                return result;
            }

            public async Task<Attachment> CreateAttachmentAsync(string tournament, int matchId,
                /*FileStream file = null,*/ string url = null, string description = null)
            {
                if (/*file == null &&*/ url == null && description == null)
                    throw new ArgumentNullException("At least 1 of the 2 optional parameters must be provided.");

                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments.json";
                
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = apiKey
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

                AttachmentData attachmentData = await PostAsync<AttachmentData>(httpClient, request, content);
                return attachmentData.Attachment;
            }

            public async Task<Attachment> GetAttachmentAsync(string tournament, int matchId, int attachmentId)
            {
                string request = $" https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments/{attachmentId}.?api_key={apiKey}";

                AttachmentData attachmentData = await GetAsync<AttachmentData>(httpClient, request);
                return attachmentData.Attachment;
            }

            public async Task<Attachment> UpdateAttachmentAsync(string tournament, int matchId, int attachmentId,
                string url = null, string description = null)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments/{attachmentId}.json";

                if (url == null && description == null)
                    throw new ArgumentNullException("At least 1 of the 3 optional parameters must be provided.");

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    ["api_key"] = apiKey
                };

                if (url != null)
                    parameters["match_attachment[url]"] = url;

                if (description != null)
                    parameters["match_attachment[description]"] = description;

                FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);

                AttachmentData attachmentData = await PutAsync<AttachmentData>(httpClient, request, content);
                return attachmentData.Attachment;
            }

            public async Task DeleteAttachmentAsync(string tournament, int matchId, int attachmentId)
            {
                string request = $"https://api.challonge.com/v1/tournaments/{tournament}/matches/{matchId}/attachments/{attachmentId}.json?api_key={apiKey}";

                await DeleteAsync(httpClient, request);
            }
        }
    }
}
