using Newtonsoft.Json;
using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint.Factories
{

    public static class HttpRequestFactory
    {
        public static async Task<HttpRequestMessage> CreateRequestAsync(UserHttpRequest userHttpRequest)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(userHttpRequest.Url),
            };

            switch (userHttpRequest.AuthenticationConfig.AuthenticationType)
            {
                case HttpAuthenticationType.None:
                    break;
                case HttpAuthenticationType.OAuth20:
                    requestMessage = await CreateRequestWithOAuth20Async(requestMessage, (OAuth20AuthenticationConfig)userHttpRequest.AuthenticationConfig.AuthenticationOptions);
                    break;
                default:
                    break;
            }

            return requestMessage;
        }

        private static async Task<HttpRequestMessage> CreateRequestWithOAuth20Async(HttpRequestMessage httpRequestMessage, OAuth20AuthenticationConfig authConfig)
        {
            var httpContent = HttpContentFactory.CreateContent(authConfig.HttpRequestConfig.ContentBody);


            HttpRequestMessage oAuth20Request = new HttpRequestMessage
            {
                Content = httpContent,
                Method = HttpMethod.Post,
                RequestUri = new Uri(authConfig.HttpRequestConfig.AuthUrl),
            };

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.SendAsync(oAuth20Request).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            OAuth20AuthResponse oAuthTokens = JsonConvert.DeserializeObject<OAuth20AuthResponse>(content);

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(authConfig.HttpRequestConfig.AuthUrl),
               
            };

            return request;
        }
    }
}
