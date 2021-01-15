using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Nimb3s.Automaton.Job.Endpoint.Factories
{

    public static class HttpOAuth20ContentFactory
    {
        public static HttpContent Create(OAuth20AuthenticationConfig oAuthConfig)
        {
            if(oAuthConfig.HttpRequestConfig == null)
            {
                throw new ArgumentNullException(nameof(oAuthConfig.HttpRequestConfig), $"You are attempting to create {Constants.OAuth20.OAUTH_20} {nameof(HttpContent)} but the {nameof(oAuthConfig.HttpRequestConfig)} is null");
            }

            HttpContent httpContent = null;

            switch (oAuthConfig.HttpRequestConfig.ContentType)
            {
                case HttpRequestContentType.ApplicationFormUrlEncoded:
                    httpContent = CreateFormUrlEncodedContent(oAuthConfig);
                    break;
                default:
                    break;
            }

            return httpContent;
        }

        private static HttpContent CreateFormUrlEncodedContent(OAuth20AuthenticationConfig oAuthConfig)
        {
             var formKeyValuePairs = OAuth20FormUrlEncodedGrantFactory.Create(oAuthConfig.Grant);

            return new FormUrlEncodedContent(formKeyValuePairs);
        }
    }
}
