using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Challonge
{
    internal static class ErrorHandler
    {
        private class ErrorData
        {
            [JsonPropertyName("errors")]
            public string[] Errors { get; set; }
        }

        /// <summary>
        /// Checks if the request returned an error or not. If not, returns the content of the reponse.
        /// </summary>
        /// <param name="response">Response to the http request</param>
        /// <returns>The content of the request if it was successful</returns>
        /// <exception cref="ChallongeException">Throws this exception if the request was unsuccessful</exception>
        /// <exception cref="ChallongeValidationException">Throws this exception if the request was successful, but not the api validation</exception>
        async static public Task<string> ParseResponseAsync(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                return responseString;

            byte[] bytes = Encoding.UTF8.GetBytes(responseString);
            MemoryStream stream = new MemoryStream(bytes);
            ErrorData errorData = await JsonSerializer.DeserializeAsync<ErrorData>(stream);

            string errors = "";
            for (int i = 0; i < errorData.Errors.Length; i++)
            {
                errors += errorData.Errors[i];
                if (i < errorData.Errors.Length - 1)
                    errors += " ; ";
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:   // Unauthorized (Invalid API key or insufficient permissions)
                    throw new ChallongeException("Challonge api request returned 401: Unauthorized. (Invalid API key or insufficient permissions). " + errors);
                case HttpStatusCode.NotFound:       // Not found
                    throw new ChallongeException("Challonge api request returned 404: Object not found. " + errors);
                case HttpStatusCode.NotAcceptable:  // Request format not supported
                    throw new ChallongeException("Challonge api request returned 406: Requested format is not supported - request JSON or XML only. " + errors);
                case HttpStatusCode.InternalServerError:   // Server-side error
                    throw new ChallongeException("Challonge api request returned 500: Something went wrong the server side. If you continually receive this, please contact the challonge team. " + errors);
                case HttpStatusCode.UnprocessableEntity:   // Validation error
                    string errorMessage = "Challonge api request returned 422: The following errors happend while trying to reach the Challonge API:\n" + errors;
                    throw new ChallongeException(errorMessage);
                default:    // We assume that by default, the request was successful
                    return responseString;
            }
        }
    }

    public class ChallongeException : Exception
    {
        public ChallongeException() { }

        public ChallongeException(string message) : base(message) { }
    }
}
