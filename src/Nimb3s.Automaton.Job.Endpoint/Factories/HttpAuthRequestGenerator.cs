using Newtonsoft.Json;
using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint.Factories
{
    public class HttpAuthRequest
    {
        public HttpRequestMessage HttpRequestMessage { get; set; }
        public AuthResponseBase AuthResponse { get; set; }
    }

    public static class HttpAuthRequestGenerator
    {
        public static async Task<HttpAuthRequest> Create(UserHttpRequest userHttpRequest)
        {
            AuthResponseBase authResponse = null;

            HttpRequestMessage httpRequest = new HttpRequestMessage
            { 
                RequestUri = new Uri(userHttpRequest.Url)
            };

            switch (userHttpRequest.AuthenticationConfig.AuthenticationType)
            {
                case HttpAuthenticationType.None:
                    break;
                case HttpAuthenticationType.OAuth20:
                    authResponse = await SetRequestWithOAuth20Async(httpRequest, (OAuth20AuthenticationConfig)userHttpRequest.AuthenticationConfig.AuthenticationOptions);
                    break;
                default:
                    break;
            }

            return new HttpAuthRequest
            {
                HttpRequestMessage = httpRequest,
                AuthResponse = authResponse
            };
        }

        public static async Task SignOut(OAuth20AuthenticationConfig authConfig, OAuth20AuthResponse authResponse)
        {
            HttpRequestMessage revocationRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(authConfig.HttpRequestConfig.RevocationUrl)
            };

            var formKeyValuePairs = OAuth20FormUrlEncodedGenerator.CreateUsingRevocation(authConfig, authResponse);

            revocationRequest.Content = new FormUrlEncodedContent(formKeyValuePairs);

            AddDefaultHeaders(revocationRequest);

            var httpClient = GetHttpClient();
            var revocationResponse = await httpClient.SendAsync(revocationRequest);

            var authResponseContent = await revocationResponse.Content.ReadAsStringAsync();

            if (revocationResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{Constants.OAuth20.OAUTH_20} failed to authenticate. Trying to access resource: {revocationRequest.RequestUri}, HttpStatusCode: {revocationResponse.StatusCode}, Reason: {authResponseContent}, Configuration: {JsonConvert.SerializeObject(authConfig)}");
            }
        }

        private static async Task<OAuth20AuthResponse> SetRequestWithOAuth20Async(HttpRequestMessage httpRequest, OAuth20AuthenticationConfig authConfig)
        {
            var oauthTokenResponse = await AuthenticateUsingOAuth20Async(httpRequest, authConfig);

            httpRequest.Headers.TryAddWithoutValidation($"{Constants.OAuth20.AUTHORIZATION}", $"{Constants.OAuth20.AUTHORIZATION_BEARER} {oauthTokenResponse.access_token}");

            return oauthTokenResponse;
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

            AddDefaultHeaders(authRequest);

            var httpClient = GetHttpClient();
            var authResponse = await httpClient.SendAsync(authRequest);

            var authResponseContent = await authResponse.Content.ReadAsStringAsync();

            if (authResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{Constants.OAuth20.OAUTH_20} failed to authenticate. Trying to access resource: {httpRequest.RequestUri}, HttpStatusCode: {authResponse.StatusCode}, Reason: {authResponseContent}, Configuration: {JsonConvert.SerializeObject(authConfig)}");
            }

            OAuth20AuthResponse oAuthTokens = JsonConvert.DeserializeObject<OAuth20AuthResponse>(authResponseContent);

            return oAuthTokens;
        }

        private static void AddDefaultHeaders(HttpRequestMessage authRequest)
        {
            authRequest.Headers.TryAddWithoutValidation("Accept", "*/*");
            authRequest.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
            authRequest.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
            authRequest.Headers.TryAddWithoutValidation("Connection", "keep-alive");
        }

        private static HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();

            handler.AllowAutoRedirect = true;
            handler.UseProxy = false;
            handler.CheckCertificateRevocationList = false;

            return new HttpClient(handler);
        }
    }
}
