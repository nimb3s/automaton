using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public static class Constants
    {
        public static class Http
        {
            public const string DEFAULT_USER_AGENT = "Automaton";
            public const string HTTP_METHOD_GET = "get";
            public const string HTTP_METHOD_POST = "post";
            public const string HTTP_METHOD_DELETE = "delete";
            public const string HTTP_METHOD_PUT = "put";

        }

        public static class OAuth20
        {
            public const string OAUTH_20 = "OAuth 2.0";

            //grants
            public const string GRANT_TYPE = "grant_type";
            public const string GRANT_TYPE_CLIENT_CREDENTIALS = "client_credentials";
            public const string GRANT_TYPE_PASSWORD = "password";

            //form url keys
            public const string CLIENT_ID = "client_id";
            public const string CLIENT_SECRET = "client_secret";
            public const string USERNAME = "username";
            public const string PASSWORD = "password";
            public const string SCOPE = "scope";

            public const string AUTHORIZATION = "Authorization";
            public const string AUTHORIZATION_BEARER = "Bearer";
        }
    }
}
