using Nimb3s.Automaton.Pocos;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Job.Endpoint.Factories
{
    public class OAuth20FormUrlEncodedFactory
    {
        public static IEnumerable<KeyValuePair<string,string>> CreateUsingGrantType(OAuth20GrantBase grant)
        {
            IEnumerable<KeyValuePair<string, string>> formKeyValuePairs = null;

            switch (grant.GrantType)
            {
                case GrantType.ClientCredentials:
                    formKeyValuePairs = CreateUsingClientGrantType((OAuth20ClientGrant)grant);
                    break;
                case GrantType.Password:
                    formKeyValuePairs = CreateUsingPasswordGrantType((OAuth20PasswordGrant)grant);
                    break;
                default:
                    break;
            }

            return formKeyValuePairs;
        }

        public static IEnumerable<KeyValuePair<string,string>> CreateUsingRevocation(OAuth20AuthenticationConfig authConfig, OAuth20AuthResponse authResponse)
        {
            var formKeyValuePairs = new List<KeyValuePair<string, string>>();

            ValidateDefaultFormValues(authConfig.Grant);

            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.CLIENT_ID, authConfig.Grant.ClientId));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.CLIENT_SECRET, authConfig.Grant.ClientSecret));

            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.TOKEN, authResponse.refresh_token));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.TOKEN_TYPE_HYNT, Constants.OAuth20.TOKEN_TYPE_HYNT_REFRESH_TOKEN));

            return formKeyValuePairs;
        }

        private static IEnumerable<KeyValuePair<string, string>> CreateUsingClientGrantType(OAuth20ClientGrant grant)
        {
            List<KeyValuePair<string, string>> formKeyValuePairs = new List<KeyValuePair<string, string>>();

            ValidateDefaultFormValues(grant);

            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.GRANT_TYPE, Constants.OAuth20.GRANT_TYPE_CLIENT_CREDENTIALS));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.CLIENT_ID, grant.ClientId));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.CLIENT_SECRET, grant.ClientSecret));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.SCOPE, grant.Scopes));

            return formKeyValuePairs;
        }

        private static IEnumerable<KeyValuePair<string, string>> CreateUsingPasswordGrantType(OAuth20PasswordGrant grant)
        {
            List<KeyValuePair<string, string>> formKeyValuePairs = new List<KeyValuePair<string, string>>();

            ValidateDefaultFormValues(grant);
            ValidatePasswordFormValues(grant);

            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.GRANT_TYPE, Constants.OAuth20.PASSWORD));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.CLIENT_ID, grant.ClientId));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.CLIENT_SECRET, grant.ClientSecret));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.USERNAME, grant.Username));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.PASSWORD, grant.UserPassword));
            formKeyValuePairs.Add(new KeyValuePair<string, string>(Constants.OAuth20.SCOPE, grant.Scopes));

            return formKeyValuePairs;
        }

        private static void ValidatePasswordFormValues(OAuth20PasswordGrant grant)
        {
            if (grant.Username == null)
            {
                throw new ArgumentNullException(nameof(grant.Username), $"You are attempting to use {Constants.OAuth20.OAUTH_20} grant type {Constants.OAuth20.GRANT_TYPE_PASSWORD} but the {nameof(grant.Username)} is null");
            }

            if (grant.UserPassword == null)
            {
                throw new ArgumentNullException(nameof(grant.UserPassword), $"You are attempting to use {Constants.OAuth20.OAUTH_20} grant type {Constants.OAuth20.GRANT_TYPE_PASSWORD} but the {nameof(grant.UserPassword)} is null");
            }
        }

        private static void ValidateDefaultFormValues(OAuth20GrantBase grant)
        {
            if (grant.ClientId == null)
            {
                throw new ArgumentNullException(nameof(grant.ClientId), $"You are attempting to use {Constants.OAuth20.OAUTH_20} grant type {Constants.OAuth20.GRANT_TYPE_CLIENT_CREDENTIALS} but the {nameof(grant.ClientId)} is null");
            }

            if (grant.ClientSecret == null)
            {
                throw new ArgumentNullException(nameof(grant.ClientSecret), $"You are attempting to use {Constants.OAuth20.OAUTH_20} grant type {Constants.OAuth20.GRANT_TYPE_CLIENT_CREDENTIALS} but the {nameof(grant.ClientSecret)} is null");
            }

            if (grant.Scopes == null)
            {
                throw new ArgumentNullException(nameof(grant.Scopes), $"You are attempting to use {Constants.OAuth20.OAUTH_20} grant type {Constants.OAuth20.GRANT_TYPE_PASSWORD} but the {nameof(grant.Scopes)} list is null");
            }
        }
    }
}
