using NServiceBus;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserSubmittedWorkItemMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public WorkItemStatus WorkItemStatus { get; set; }
        public IEnumerable<Request> HttpRequests { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }

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
}
