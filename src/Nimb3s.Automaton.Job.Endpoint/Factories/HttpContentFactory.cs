using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Nimb3s.Automaton.Job.Endpoint.Factories
{

    public static class HttpContentFactory
    {
        public static HttpContent CreateContent(HttpRequestContentBase httpRequestContent)
        {
            HttpContent httpContent = null;

            switch (httpRequestContent.ContentType)
            {
                case HttpRequestContentType.ApplicationFormUrlEncoded:
                    httpContent = CreateFormUrlEncodedContent((ApplicationFormUrlEncodedContent) httpRequestContent);
                    break;
                default:
                    break;
            }

            return httpContent;
        }

        private static HttpContent CreateFormUrlEncodedContent(ApplicationFormUrlEncodedContent httpRequestContent)
        {
            if (httpRequestContent.NameValueCollection == null)
            {
                throw new NullReferenceException($"You are attempting to use application/x-www-form-urlencoded the given {nameof(httpRequestContent.NameValueCollection)} is null");
            }

            return new FormUrlEncodedContent(httpRequestContent.NameValueCollection);
        }
    }
}
