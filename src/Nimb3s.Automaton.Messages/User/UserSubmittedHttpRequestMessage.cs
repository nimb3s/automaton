﻿using NServiceBus;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserSubmittedHttpRequestMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public UserHttpRequest HttpRequest { get; set; }
    }

    /// <summary>
    /// The http request to be executed as part of an automation job work item.
    /// </summary>
    public class UserHttpRequest
    {
        /// <summary>
        /// The HTTP request id
        /// </summary>
        public Guid HttpRequestId { get; set; }

        public string Url { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> ContentHeaders { get; set; }
        public HttpAuthenticationConfig AuthenticationConfig { get; set; }
    }

    public enum HttpRequestStatusType
    {
        Queued = 1,
        Started = 2,
        Completed = 3
    }

    public class HttpAuthenticationConfig
    {
        public HttpAuthenticationType AuthenticationType { get; set; }
        public HttpAuthenticationOptions AuthenticationOptions { get; set; }
    }

    public enum HttpAuthenticationType
    {
        None = 1,
        OAuth20 = 2
    }

    public class HttpAuthenticationOptions
    {

    }

    #region OAuth20 Auth Config
    public class OAuth20AuthenticationConfig : HttpAuthenticationOptions
    {
        public GrantType GrantType { get; }

        public OAuth20Grant Grant { get; set; }

        public string Scopes { get; set; }
    }

    public enum GrantType
    {
        /// <summary>
        /// Supports client id and client secret: client_credentials
        /// </summary>
        ClientCredentials = 1,
        /// <summary>
        /// Supports client id and client secret with username and user password: password
        /// </summary>
        Password = 2
    }

    public class OAuth20Grant
    {

    }

    public class OAuth20ClientGrant : OAuth20Grant
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class OAuth20PasswordGrant : OAuth20Grant
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
    }
    #endregion
}
