using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Api.Models
{
    /// <summary>
    /// The HTTP request execution status. If request started or comleted execution.
    /// </summary>
    public enum WorkItemHttpRequestStatus
    {
        Started,
        Completed,
    }

    /// <summary>
    /// The http request to be executed as part of an automation job work item.
    /// </summary>
    public class WorkItemHttpRequestModel
    {
        /// <summary>
        /// The HTTP request id
        /// </summary>
        public string HttpRequestId { get; set; }

        /// <summary>
        /// The HTTP request status.
        /// </summary>
        public WorkItemHttpRequestStatus WorkItemHttpRequestStatus { get; set; }
    }
}
