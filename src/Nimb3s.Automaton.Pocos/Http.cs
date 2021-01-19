using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Pocos
{
    /// <summary>
    /// The <see cref="WorkItemModel"/> status. When a work item starts or finishes.
    /// </summary>

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
        public string UserAgent { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> ContentHeaders { get; set; }
        public HttpAuthenticationConfig AuthenticationConfig { get; set; }
    }

    public class HttpAuthenticationConfig
    {
        public HttpAuthenticationType AuthenticationType { get; set; }
        public HttpAuthenticationOptions AuthenticationOptions { get; set; }
    }

    public class HttpAuthenticationOptions
    {

    }
}
