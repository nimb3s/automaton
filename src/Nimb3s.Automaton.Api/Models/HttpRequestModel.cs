using Nimb3s.Automaton.Messages;
using Nimb3s.Automaton.Messages.HttpRequests;
using Nimb3s.Automaton.Messages.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Api.Models
{
    public class HttpRequestModel
    {
        /// <summary>
        /// The HTTP request id
        /// </summary>
        public Guid HttpRequestId { get; set; }

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

        /// <summary>
        /// The HTTP request status.
        /// </summary>
        public WorkItemStatus WorkItemStatus { get; set; }
    }
}
