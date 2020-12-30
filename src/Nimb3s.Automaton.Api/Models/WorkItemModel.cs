using Nimb3s.Automaton.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nimb3s.Automaton.Api.Models
{
    /// <summary>
    /// An automation job work item. A work item is composed of one or more HTTP Requests that neeed to be executed
    /// </summary>
    public class WorkItemModel
    {
        /// <summary>
        /// The automation job id.
        /// </summary>
        [Required]
        public Guid AutomationJobId { get; set; }

        /// <summary>
        /// The work item id.
        /// </summary>
        public Guid WorkItemId { get; set; }


        /// <summary>
        /// The work item status. When a workitem Starts and finishes.
        /// </summary>
        public WorkItemStatus WorkItemStatus { get; set; }

        /// <summary>
        /// The collection of <see cref="HttpRequest"/> to exeute.
        /// </summary>
        //[Required]
        public IEnumerable<HttpRequestModel> HttpRequests { get; set; }
    }
}
