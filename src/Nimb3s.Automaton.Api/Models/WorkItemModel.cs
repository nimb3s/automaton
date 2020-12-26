using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Api.Models
{

    /// <summary>
    /// The <see cref="WorkItemModel"/> status. When a work item starts or finishes.
    /// </summary>
    public enum WorkItemStatus
    {
        /// <summary>
        /// When a work item is queued.
        /// </summary>
        Queued,

        /// <summary>
        /// When a work item starts running.
        /// </summary>
        Started,

        /// <summary>
        /// When a work item completes.
        /// </summary>
        Completed
    }


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
        /// The collection of <see cref="WorkItemHttpRequestModel"/> to exeute.
        /// </summary>
        [Required]
        public IEnumerable<WorkItemHttpRequestModel> HttpRequests { get; set; }
    }
}
