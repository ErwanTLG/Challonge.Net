using System;
using System.Net.Http;
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
            switch ((int)response.StatusCode)
            {
                case 200:   // OK
                    return await response.Content.ReadAsStringAsync();
                case 401:   // Unauthorized (Invalid API key or insufficient permissions)
                    throw new ChallongeException("Challonge api request returned 401: Unauthorized. (Invalid API key or insufficient permissions)");
                case 404:   // Not found
                    throw new ChallongeException("Challonge api request returned 404: Object not found.");
                case 406:   // Request format not supported
                    throw new ChallongeException("Challonge api request returned 406: Requested format is not supported - request JSON or XML only");
                case 500:   // Server-side error
                    throw new ChallongeException("Challonge api request returned 500: Something went wrong the server side. If you continually receive this, please contact the challonge team.");
                case 422:   // Validation error
                    string responseString = await response.Content.ReadAsStringAsync();
                    ErrorData errorData = JsonSerializer.Deserialize<ErrorData>(responseString);

                    string errorMessage = "The following errors happend while trying to reach the Challonge API:\n";
                    foreach (string message in errorData.Errors)
                        errorMessage += message + "\n";
                    throw new ChallongeValidationException(errorMessage);
                default:    // We assume that by default, the request was successful
                    return await response.Content.ReadAsStringAsync();
            }
        }
    }

    public class ChallongeException : Exception
    {
        public ChallongeException() { }

        public ChallongeException(string message) : base(message) { }
    }

    public class ChallongeValidationException : ChallongeException
    {
        public ChallongeValidationException() { }

        public ChallongeValidationException(string message) : base(message) { }
    }
}
