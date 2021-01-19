using System;

namespace Nimb3s.Automaton.Pocos.Models
{
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
