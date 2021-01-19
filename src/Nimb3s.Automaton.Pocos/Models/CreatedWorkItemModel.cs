using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Pocos.Models
{
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
