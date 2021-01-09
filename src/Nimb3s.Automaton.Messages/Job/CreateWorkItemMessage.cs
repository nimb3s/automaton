using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Messages.Job
{
    public class CreateWorkItemMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public WorkItemStatus WorkItemStatus { get; set; }
        public IEnumerable<Request> HttpRequests { get; set; }
    }
}
