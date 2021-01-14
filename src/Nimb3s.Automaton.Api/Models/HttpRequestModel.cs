using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nimb3s.Automaton.Api.Models
{
    public class HttpRequestBaseModel
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public string Method { get; set; }
        public string Content { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> ContentHeaders { get; set; }
        [Required]
        public HttpAuthenticationConfig AuthenticationConfig { get; set; }
    }

    public class NewHttpRequestModel : HttpRequestBaseModel
    {

    }

    public class CreatedHttpRequestModel : HttpRequestBaseModel
    {
        /// <summary>
        /// The HTTP request id
        /// </summary>
        public Guid HttpRequestId { get; set; }

        /// <summary>
        /// The HTTP request status.
        /// </summary>
        public HttpRequestStatusType HttpRequestStatus { get; set; }
    }
}
