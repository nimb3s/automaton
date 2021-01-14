using NServiceBus;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserCreatedWorkItemMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public IEnumerable<UserHttpRequest> HttpRequests { get; set; }
        public DateTimeOffset DateActionTaken { get; set; }
    }

    /// <summary>
    /// The <see cref="WorkItemModel"/> status. When a work item starts or finishes.
    /// </summary>
    public enum WorkItemStatusType
    {
        /// <summary>
        /// When a work item is queued.
        /// </summary>
        Queued = 1,

        /// <summary>
        /// When a work item starts running.
        /// </summary>
        Started = 2,

        /// <summary>
        /// When a work item starts running.
        /// </summary>
        ReStarted = 3,

        /// <summary>
        /// When a work item completes.
        /// </summary>
        Completed = 4
    }
}
