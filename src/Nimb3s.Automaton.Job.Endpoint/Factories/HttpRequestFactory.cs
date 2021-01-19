using Newtonsoft.Json;
using Nimb3s.Automaton.Pocos;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint.Factories
{

    public static class HttpRequestFactory
    {
        public static async Task<HttpRequestMessage> Create(UserHttpRequest userHttpRequest)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage
            { 
                RequestUri = new Uri(userHttpRequest.Url)
            };

            switch (userHttpRequest.AuthenticationConfig.AuthenticationType)
            {
                case HttpAuthenticationType.None:
                    break;
                case HttpAuthenticationType.OAuth20:
                    await SetRequestWithOAuth20Async(httpRequest, (OAuth20AuthenticationConfig)userHttpRequest.AuthenticationConfig.AuthenticationOptions);
                    break;
                default:
                    break;
            }

            return httpRequest;
        }

        private static async Task<HttpRequestMessage> SetRequestWithOAuth20Async(HttpRequestMessage httpRequest, OAuth20AuthenticationConfig authConfig)
        {
            var oauthTokenResponse = await AuthenticateUsingOAuth20Async(httpRequest, authConfig);

            httpRequest.Headers.TryAddWithoutValidation($"{Constants.OAuth20.AUTHORIZATION}", $"{Constants.OAuth20.AUTHORIZATION_BEARER} {oauthTokenResponse.access_token}");

            return httpRequest;
        }

        private static async Task<OAuth20AuthResponse> AuthenticateUsingOAuth20Async(HttpRequestMessage httpRequest, OAuth20AuthenticationConfig authConfig)
        {
            var authRequestContent = HttpOAuth20ContentFactory.Create(authConfig);

            HttpRequestMessage authRequest = new HttpRequestMessage
            {
                Content = authRequestContent,
                Method = HttpMethod.Post,
                RequestUri = new Uri(authConfig.HttpRequestConfig.AuthUrl),
            };

            authRequest.Headers.UserAgent.TryParseAdd("PostmanRuntime/7.26.8");

            authRequest.Headers.TryAddWithoutValidation("Accept", "*/*");
            authRequest.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
            authRequest.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
            authRequest.Headers.TryAddWithoutValidation("Connection", "keep-alive");

            var handler = new HttpClientHandler();

            handler.AllowAutoRedirect = true;
            handler.UseProxy = false;
            handler.CheckCertificateRevocationList = false;

            var httpClient = new HttpClient(handler);
            var authResponse = await httpClient.SendAsync(authRequest);

            var authResponseContent = await authResponse.Content.ReadAsStringAsync();

            if (authResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{Constants.OAuth20.OAUTH_20} failed to authenticate. Trying to access resource: {httpRequest.RequestUri}, HttpStatusCode: {authResponse.StatusCode}, Reason: {authResponseContent}, Configuration: {JsonConvert.SerializeObject(authConfig)}");
            }

            OAuth20AuthResponse oAuthTokens = JsonConvert.DeserializeObject<OAuth20AuthResponse>(authResponseContent);

            return oAuthTokens;
        }
    }
}
