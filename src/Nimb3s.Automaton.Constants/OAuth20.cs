using System;

namespace Nimb3s.Automaton.Constants
{
    public static partial class AutomatonConstants
    {
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

            public const string TOKEN = "token";
            public const string TOKEN_TYPE_HYNT = "token_type_hint";
            public const string TOKEN_TYPE_HYNT_REFRESH_TOKEN = "refresh_token";
        }
    }
}
