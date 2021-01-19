using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nimb3s.Automaton.Api.Models
{
    public class WorkItemModelBase
    {
        /// <summary>
        /// The automation job id.
        /// </summary>
        [Required]
        public Guid JobId { get; set; }

    }

    /// <summary>
    /// An automation job work item. A work item is composed of one or more HTTP Requests that neeed to be executed
    /// </summary>
    public class NewWorkItemModel : WorkItemModelBase
    {
        /// <summary>
        /// The collection of <see cref="NewHttpRequestModel"/> to exeute.
        /// </summary>
        //[Required]
        public IEnumerable<NewHttpRequestModel> HttpRequests { get; set; }
    }

    public class CreatedWorkItemModel : WorkItemModelBase
    {
        /// <summary>
        /// The work item id.
        /// </summary>
        public Guid WorkItemId { get; set; }

        /// <summary>
        /// The work item status. When a workitem Starts and finishes.
        /// </summary>
        public WorkItemStatusType WorkItemStatus { get; set; }

        /// <summary>
        /// The collection of <see cref="CreatedHttpRequestModel"/> to exeute.
        /// </summary>
        //[Required]
        public IEnumerable<CreatedHttpRequestModel> HttpRequests { get; set; }
    }
}
